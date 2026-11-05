
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Atividade_3.DTO;
using Atividade_3.Models;

namespace AplicacaoBackAula.Controllers
{
  //Configuração da API
  [ApiController]
  [Route("api/[controller]")]
  public class PessoaEnderecoController : ControllerBase
  {
    //Declaração do atributo do CONTEXT
    private readonly BdAulaBackendContext _context;

    //Declaração do método construtor 
    public PessoaEnderecoController(BdAulaBackendContext context)
    {
      //Instanciando o atributo context
      _context = context;
    }

    //Listar todas as pessoas com seus respectivos endereçõs


    [HttpGet]
    public async Task<ActionResult<IEnumerable<PessoaDTO>>> GetAll()
    {
      var pessoas = await _context.Pessoas.Select(p => new PessoaDTO
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
          IdEndereco = e.Idendereco,
          IdPessoa = e.Idpessoa,
          Cep = e.Cep,
          Numero = e.Numero,
          Complemento = e.Complemento,
        }).ToList()
      }).ToListAsync();

      return Ok(pessoas);
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<PessoaDTO>> GetPessoa(int id)
    {
      var pessoa = await _context.Pessoas.Where(p => p.Idpessoa == id).Select(p => new PessoaDTO
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
          Complemento = e.Complemento,
        }).ToList()
      }).FirstOrDefaultAsync();

      return Ok(pessoa);
    }

    [HttpPost]
    public async Task<ActionResult<PessoaDTO>> Post(PessoaDTO dto)
    {

      var ObjPessoa = new Pessoa
      {
        Nome = dto.Nome,
        Cpf = (long)dto.Cpf,
        DataNascimento = dto.DataNascimento,
        Sexo = dto.Sexo,
        Email = dto.Email,
        Telefone = dto.Telefone,
        AtivoPessoa = dto.AtivoPessoa = "A",

        Enderecos = dto.Enderecos.Select(e => new Endereco
        {
          Cep = e.Cep,
          Numero = e.Numero,
          Complemento = e.Complemento
        }).ToList()
      };
      _context.Pessoas.Add(ObjPessoa);

      await _context.SaveChangesAsync();

      var respostaBd = new PessoaDTO
      {
        IdPessoa = ObjPessoa.Idpessoa,
        Nome = ObjPessoa.Nome,
        Cpf = ObjPessoa.Cpf,
        DataNascimento = ObjPessoa.DataNascimento,
        Sexo = ObjPessoa.Sexo,
        Email = ObjPessoa.Email,
        Telefone = ObjPessoa.Telefone,
        Enderecos = ObjPessoa.Enderecos.Select(e => new EnderecoDTO
        {
          IdPessoa = e.Idpessoa,
          Numero = e.Numero,
          Cep = e.Cep,
          Complemento = e.Complemento
        }).ToList()
      };

      return CreatedAtAction(
          nameof(GetPessoa),
          new { id = ObjPessoa.Idpessoa }, respostaBd

          );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ActionResult<Pessoa>>> Put(int id, PessoaDTO dto)
    {
      var pessoa = await _context.Pessoas.Include(p => p.Enderecos).FirstOrDefaultAsync(p => p.Idpessoa == id);

      if (pessoa == null)
      {
        return NotFound();
      }
      //Procu
      pessoa.Nome = dto.Nome;
      pessoa.Cpf = (long)dto.Cpf;
      pessoa.DataNascimento = dto.DataNascimento;
      pessoa.Sexo = dto.Sexo;
      pessoa.Email = dto.Email;
      pessoa.Telefone = dto.Telefone;


      var idRecebidos = dto.Enderecos.Where(e => e.IdEndereco.HasValue).Select(e => e.IdEndereco!.Value).ToHashSet();

      var enderecosRemover = pessoa.Enderecos.Where(e => !idRecebidos.Contains(e.Idendereco)).ToList();

      _context.Enderecos.RemoveRange(enderecosRemover);
      foreach (var endPessoa in dto.Enderecos)
      {
        if (endPessoa.IdPessoa > 0)
        {
          var endereco = pessoa.Enderecos.FirstOrDefault(e => e.Idpessoa == id);

          if (endereco != null)
          {
            return BadRequest($"O endereço não pertence a essa pessoa");
          }

          endereco.Numero = endPessoa.Numero;
          endereco.Cep = endPessoa.Cep;
          endereco.Complemento = endPessoa.Complemento;
        }
        else
        {
          pessoa.Enderecos.Add(new Endereco
          {
            Numero = endPessoa.Numero,
            Cep = endPessoa.Cep,
            Complemento = endPessoa.Complemento
          });
        }
      }
      await _context.SaveChangesAsync();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Pessoa>> Delete(int id)
    {
      var pessoa = await _context.Pessoas.Include(p => p.Enderecos).FirstOrDefaultAsync(p => p.Idpessoa == id);

      if (pessoa == null)
      {
        return NotFound();
      }

      _context.Pessoas.Remove(pessoa);
      await _context.SaveChangesAsync();

      return NoContent();
    }
  }
}
