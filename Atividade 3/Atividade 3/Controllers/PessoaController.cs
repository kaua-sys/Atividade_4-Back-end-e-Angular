
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Atividade_3.DTO;
using Atividade_3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Atividade_3.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PessoaController : ControllerBase
  {
    private readonly BdAulaBackendContext _context;

    public PessoaController(BdAulaBackendContext context)
    {
      _context = context;
    }

    // GET: api/Pessoa
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PessoaDTO>>> GetPessoa()
    {
      var pessoas = await _context.Pessoas
          .Where(p => p.AtivoPessoa == "A")
          .Select(p => new PessoaDTO
          {
            IdPessoa = p.Idpessoa,
            Nome = p.Nome,
            Cpf = p.Cpf,
            DataNascimento = p.DataNascimento,
            Sexo = p.Sexo,
            Email = p.Email,
            Telefone = p.Telefone,

            Enderecos = p.Enderecos.Select(e => new EnderecoDTO
            {
              IdPessoa = e.Idpessoa,
              IdEndereco = e.Idendereco,
              Cep = e.Cep,
              Numero = e.Numero,
              Complemento = e.Complemento
            }).ToList()
          })
          .ToListAsync();

      return Ok(pessoas);
    }

    // GET: api/Pessoa/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PessoaDTO>> GetPessoa(int id)
    {
      var pessoa = await _context.Pessoas
          .Where(p => p.Idpessoa == id)
          .Select(p => new PessoaDTO
          {
            IdPessoa = p.Idpessoa,
            Nome = p.Nome,
            Cpf = p.Cpf,
            DataNascimento = p.DataNascimento,
            Sexo = p.Sexo,
            Email = p.Email,
            Telefone = p.Telefone,

            Enderecos = p.Enderecos.Select(e => new EnderecoDTO
            {
              IdPessoa = e.Idpessoa,
              IdEndereco = e.Idendereco,
              Cep = e.Cep,
              Numero = e.Numero,
              Complemento = e.Complemento
            }).ToList()
          })
          .FirstOrDefaultAsync();

      if (pessoa == null)
      {
        return NotFound(new
        {
          mensagem = "Pessoa não encontrada."
        });
      }

      return Ok(pessoa);
    }

    // POST: api/Pessoa
    [HttpPost]
    public async Task<ActionResult> PostPessoa(PessoaPostDTO dto)
    {
      var pessoa = new Pessoa
      {
        Nome = dto.Nome,
        Cpf = dto.Cpf,
        DataNascimento = dto.DataNascimento,
        Sexo = dto.Sexo,
        Email = dto.Email,
        Telefone = dto.Telefone,
        AtivoPessoa = dto.AtivoPessoa,
        TipoUsuario = "cliente"
      };

      var hasher = new PasswordHasher<Pessoa>();

      pessoa.SenhaHash = hasher.HashPassword(pessoa, "cliente");

      _context.Pessoas.Add(pessoa);
      await _context.SaveChangesAsync();

      return Ok(new
      {
        mensagem = "Cliente cadastrado com sucesso!",
        idPessoa = pessoa.Idpessoa
      });
    }

    // POST: api/Pessoa/login
    [HttpPost("login")]
    public async Task<ActionResult> Login(PessoaLoginDTO dto)
    {
      var pessoa = await _context.Pessoas
          .FirstOrDefaultAsync(p => p.Email == dto.Email);

      if (pessoa == null || pessoa.SenhaHash == null)
      {
        return Unauthorized(new
        {
          mensagem = "E-mail ou senha incorretos."
        });
      }

      var hasher = new PasswordHasher<Pessoa>();

      var resultado = hasher.VerifyHashedPassword(
          pessoa,
          pessoa.SenhaHash,
          dto.Senha
      );

      if (resultado == PasswordVerificationResult.Failed)
      {
        return Unauthorized(new
        {
          mensagem = "E-mail ou senha incorretos."
        });
      }

      return Ok(new
      {
        idPessoa = pessoa.Idpessoa,
        nome = pessoa.Nome,
        tipoUsuario = pessoa.TipoUsuario
      });
    }

    // POST: api/Pessoa/identificar
    [HttpPost("identificar")]
    public async Task<ActionResult> IdentificarPessoa(PessoaIdentificarDTO dto)
    {
      var pessoa = await _context.Pessoas
          .FirstOrDefaultAsync(p => p.Cpf == dto.Cpf && p.Email == dto.Email);

      if (pessoa == null)
      {
        return NotFound(new
        {
          mensagem = "CPF ou e-mail não encontrado."
        });
      }

      return Ok(new
      {
        idPessoa = pessoa.Idpessoa,
        nome = pessoa.Nome
      });
    }

    // PUT: api/Pessoa/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutPessoa(int id, PessoaPostDTO dto)
    {
      var pessoa = await _context.Pessoas
          .FirstOrDefaultAsync(p => p.Idpessoa == id);

      if (pessoa == null)
      {
        return NotFound(new
        {
          mensagem = "Pessoa não encontrada."
        });
      }

      pessoa.Nome = dto.Nome;
      pessoa.Cpf = dto.Cpf;
      pessoa.DataNascimento = dto.DataNascimento;
      pessoa.Sexo = dto.Sexo;
      pessoa.Email = dto.Email;
      pessoa.Telefone = dto.Telefone;

      await _context.SaveChangesAsync();

      return NoContent();
    }

    // DELETE: api/Pessoa/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePessoa(int id)
    {
      var pessoa = await _context.Pessoas
          .FirstOrDefaultAsync(p => p.Idpessoa == id);

      if (pessoa == null)
      {
        return NotFound(new
        {
          mensagem = "Pessoa não encontrada."
        });
      }

      var possuiEndereco = await _context.Enderecos
          .AnyAsync(e => e.Idpessoa == id);

      var possuiFuncionario = await _context.Funcionarios
          .AnyAsync(f => f.Idpessoa == id);

      var possuiAtendimento = await _context.Atendimentos
          .AnyAsync(a => a.Idpessoa == id);

      if (possuiEndereco || possuiFuncionario || possuiAtendimento)
      {
        return BadRequest(new
        {
          mensagem = "Não é possível excluir a pessoa porque existem registros vinculados a ela."
        });
      }

      _context.Pessoas.Remove(pessoa);
      await _context.SaveChangesAsync();

      return NoContent();
    }
  }
}
