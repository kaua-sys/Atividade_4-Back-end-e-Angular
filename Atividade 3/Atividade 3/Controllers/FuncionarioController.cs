using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Atividade_3.DTO;
using Atividade_3.Models;

namespace Atividade_3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionarioController : ControllerBase
    {
        // Conexão com o banco de dados
        private readonly BdAulaBackendContext _context;

        public FuncionarioController(BdAulaBackendContext context)
        {
            _context = context;
        }

        // Lista todos os funcionários com seus dados pessoais e endereços
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuncionarioDTO>>> GetAll()
        {
            var funcionarios = await _context.Funcionarios.Where(f => f.IdpessoaNavigation.AtivoPessoa == "A").Select(pe => new FuncionarioDTO
               {
                    IdPessoa = pe.Idpessoa,
                    Cargo = pe.Cargo,
                    DataAdmissao = pe.DataAdmissao,

                    // Retorna os dados da pessoa vinculada ao funcionário
                    Pessoa = new PessoaDTO()
                    {
                        Nome = pe.IdpessoaNavigation.Nome,
                        Cpf = pe.IdpessoaNavigation.Cpf,
                        Email = pe.IdpessoaNavigation.Email,
                        Sexo = pe.IdpessoaNavigation.Sexo,
                        DataNascimento = pe.IdpessoaNavigation.DataNascimento,
                        Telefone = pe.IdpessoaNavigation.Telefone,
                        AtivoPessoa = pe.IdpessoaNavigation.AtivoPessoa,

                      // Converte os endereços para DTO
                      Enderecos = pe.IdpessoaNavigation.Enderecos.Select(e => new EnderecoDTO
                        {
                            IdPessoa = e.Idpessoa,
                            IdEndereco = e.Idendereco,
                            Cep = e.Cep,
                            Numero = e.Numero,
                            Complemento = e.Complemento,
                        }).ToList()
                    }
                }).ToListAsync();

            return Ok(funcionarios);
        }

        // Busca um funcionário pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<FuncionarioDTO>> GetFuncionario(int id)
        {
            var funcionario = await _context.Funcionarios
                .Where(p => p.Idpessoa == id)
                .Select(f => new FuncionarioDTO
                {
                    IdPessoa = f.Idpessoa,
                    Cargo = f.Cargo,
                    DataAdmissao = f.DataAdmissao,

                    // Retorna os dados pessoais do funcionário
                    Pessoa = new PessoaDTO()
                    {
                        Nome = f.IdpessoaNavigation.Nome,
                        Cpf = f.IdpessoaNavigation.Cpf,
                        Email = f.IdpessoaNavigation.Email,
                        Sexo = f.IdpessoaNavigation.Sexo,
                        DataNascimento = f.IdpessoaNavigation.DataNascimento,
                        Telefone = f.IdpessoaNavigation.Telefone,

                        // Lista os endereços da pessoa
                        Enderecos = f.IdpessoaNavigation.Enderecos.Select(e => new EnderecoDTO
                        {
                            IdPessoa = e.Idpessoa,
                            IdEndereco = e.Idendereco,
                            Cep = e.Cep,
                            Numero = e.Numero,
                            Complemento = e.Complemento,
                        }).ToList()
                    }
                }).FirstOrDefaultAsync();

            // Verifica se o funcionário existe
            if (funcionario == null)
            {
                return NotFound();
            }

            return Ok(funcionario);
        }

        // Cadastra um novo funcionário
        [HttpPost]
        public async Task<ActionResult<FuncionarioDTO>> Post(FuncionarioDTO dto)
        {
            // Verifica se os dados da pessoa foram informados
            if (dto.Pessoa == null)
            {
                return BadRequest("Por favor adicione os dados para o cadastro");
            }

            // Verifica se CPF e telefone foram informados
            if (dto.Pessoa.Cpf == null || dto.Pessoa.Telefone == null)
            {
                return BadRequest("Cpf ou telefone ausente, por favor adicioneos");
            }


            // Verifica se já existe alguém com o mesmo e-mail
            if (await _context.Pessoas.AnyAsync(p => p.Email == dto.Pessoa.Email))
            {
                return Conflict("Já existe uma pessoa cadastrada com esse e-mail.");
            }

            if (await _context.Pessoas.AnyAsync(p => p.Cpf == dto.Pessoa.Cpf))
            {
                return Conflict("Já existe uma pessoa cadastrada com esse cpf.");
            }
            // Cria a pessoa com os dados recebidos
            var pessoa = new Pessoa
            {
                Nome = dto.Pessoa.Nome!,
                Cpf = dto.Pessoa.Cpf.Value,
                DataNascimento = dto.Pessoa.DataNascimento,
                Sexo = dto.Pessoa.Sexo,
                Email = dto.Pessoa.Email,
                Telefone = dto.Pessoa.Telefone.Value,
                AtivoPessoa = "A",

                // Cadastra também os endereços informados
                Enderecos = dto.Pessoa.Enderecos?.Select(e => new Endereco
                {
                    Cep = e.Cep,
                    Complemento = e.Complemento,
                    Numero = e.Numero,
                }).ToList() ?? new List<Endereco>()
            };

            // Cria o funcionário e vincula a pessoa
            var funcionario = new Funcionario
            {
                Cargo = dto.Cargo!,
                DataAdmissao = dto.DataAdmissao,
                IdpessoaNavigation = pessoa,
            };

            _context.Funcionarios.Add(funcionario);

            // Salva os dados no banco
            await _context.SaveChangesAsync();

            return Ok("Funcionário cadastrado com sucesso!");
        }

        // Atualiza os dados de um funcionário existente
        [HttpPut("{id}")]
        public async Task<ActionResult<Funcionario>> Put(int id, FuncionarioDTO dto)
        {
            // Busca o funcionário, a pessoa vinculada e seus endereços
            var funcionario = await _context.Funcionarios
                .Include(f => f.IdpessoaNavigation)
                .ThenInclude(e => e.Enderecos)
                .FirstOrDefaultAsync(f => f.Idpessoa == id);

            // Verifica se o funcionário existe
            if (funcionario == null)
            {
                return NotFound("Funcionário não encontrado.");
            }

            // Verifica se os dados da pessoa foram enviados
            if (dto.Pessoa == null)
            {
                return BadRequest("Informe os dados da pessoa.");
            }

            // Verifica se CPF e telefone foram informados
            if (dto.Pessoa.Cpf == null || dto.Pessoa.Telefone == null)
            {
                return BadRequest("CPF e telefone são obrigatórios.");
            }

            // Atualiza os dados do funcionário
            funcionario.Cargo = dto.Cargo;
            funcionario.DataAdmissao = dto.DataAdmissao;

            // Atualiza os dados da pessoa vinculada
            funcionario.IdpessoaNavigation.Nome = dto.Pessoa.Nome!;
            funcionario.IdpessoaNavigation.Cpf = dto.Pessoa.Cpf.Value;
            funcionario.IdpessoaNavigation.DataNascimento = dto.Pessoa.DataNascimento;
            funcionario.IdpessoaNavigation.Sexo = dto.Pessoa.Sexo!;
            funcionario.IdpessoaNavigation.Email = dto.Pessoa.Email!;
            funcionario.IdpessoaNavigation.Telefone = dto.Pessoa.Telefone.Value;

      // Recebe a lista de endereços enviada
      var enderecosDto = dto.Pessoa.Enderecos ?? new List<EnderecoDTO>();

            // Identifica os endereços que foram enviados com ID
            var idRecebidos = enderecosDto
                .Where(e => e.IdEndereco.HasValue)
                .Select(e => e.IdEndereco!.Value)
                .ToHashSet();

            foreach (var enderecoDto in enderecosDto)
            {
                if (enderecoDto.IdEndereco.HasValue)
                {
                    var pertence = funcionario.IdpessoaNavigation.Enderecos
                        .Any(e => e.Idendereco == enderecoDto.IdEndereco.Value);

                    if (!pertence)
                    {
                        return BadRequest(
                            "O endereço não pertence a essa pessoa.");
                    }
                }
            }
            // Separa os endereços existentes que não foram enviados para remoção
            var enderecosRemover = funcionario.IdpessoaNavigation.Enderecos
                .Where(e => !idRecebidos.Contains(e.Idendereco))
                .ToList();

            _context.Enderecos.RemoveRange(enderecosRemover);

            // Atualiza os endereços existentes ou adiciona novos
            foreach (var endPessoa in enderecosDto)
            {
                if (endPessoa.IdEndereco.HasValue)
                {
                    // Procura o endereço pelo ID
                    var endereco = funcionario.IdpessoaNavigation.Enderecos
                        .FirstOrDefault(e => e.Idendereco == endPessoa.IdEndereco.Value);

                    // Verifica se o endereço pertence à pessoa
                    if (endereco == null)
                    {
                        return BadRequest("O endereço não pertence a essa pessoa.");
                    }

                    // Atualiza o endereço encontrado
                    endereco.Numero = endPessoa.Numero!;
                    endereco.Cep = endPessoa.Cep!;
                    endereco.Complemento = endPessoa.Complemento!;
                }
                else
                {
                    // Adiciona um novo endereço à pessoa
                    funcionario.IdpessoaNavigation.Enderecos.Add(new Endereco
                    {
                        Idpessoa = funcionario.Idpessoa,
                        Numero = endPessoa.Numero!,
                        Cep = endPessoa.Cep!,
                        Complemento = endPessoa.Complemento!
                    });
                }
            }

            // Salva as alterações no banco de dados
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Exclui um funcionário pelo ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var funcionario = await _context.Funcionarios
                .Include(f => f.IdpessoaNavigation)
                .FirstOrDefaultAsync(f => f.Idpessoa == id);

            if (funcionario == null)
            {
                return NotFound("Funcionário não encontrado.");
            }

            // Marca a pessoa como inativa
            funcionario.IdpessoaNavigation.AtivoPessoa = "I";

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
