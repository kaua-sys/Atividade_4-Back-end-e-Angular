using Atividade_3.DTO;
using Atividade_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atividade_3.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EnderecoController : ControllerBase
    {
        private readonly BdAulaBackendContext _context;

        public EnderecoController(BdAulaBackendContext context)
        {
            _context = context;
        }

        // GET: api/Endereco
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Endereco>>> GetEnderecos()
        {
            return await _context.Enderecos.ToListAsync();
        }


        // GET: api/Endereco/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Endereco>> GetEndereco(int id)
        {
            var Endereco = await _context.Enderecos
                .FirstOrDefaultAsync(c => c.Idendereco == id);

            if (Endereco == null)
            {
                return NotFound();
            }

            return Endereco;
        }


        // POST: api/Endereco
        [HttpPost]
        public async Task<ActionResult> PostEndereco(EnderecoDTO dto)
        {
            var pessoaExiste = await _context.Pessoas
                .AnyAsync(p => p.Idpessoa == dto.IdPessoa);

            if (!pessoaExiste)
            {
                return BadRequest(new
                {
                    mensagem = "A pessoa informada não existe."
                });
            }

            var endereco = new Endereco
            {
                Idpessoa = dto.IdPessoa,
                Cep = dto.Cep!,
                Numero = dto.Numero!,
                Complemento = dto.Complemento!
            };

            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEndereco),
                new { id = endereco.Idendereco },
                new
                {
                    mensagem = "Endereço cadastrado com sucesso.",
                    id = endereco.Idendereco
                }
            );
        }


        // PUT: api/Endereco/5

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutEndereco(int id, EnderecoDTO dto)
        {
            var enderecoExistente = await _context.Enderecos
                .FirstOrDefaultAsync(e => e.Idendereco == id);

            if (enderecoExistente == null)
            {
                return NotFound(new
                {
                    mensagem = "Endereço não encontrado."
                });
            }

            var pessoaExiste = await _context.Pessoas
                .AnyAsync(p => p.Idpessoa == dto.IdPessoa);

            if (!pessoaExiste)
            {
                return BadRequest(new
                {
                    mensagem = "A pessoa informada não existe."
                });
            }

            enderecoExistente.Idpessoa = dto.IdPessoa;
            enderecoExistente.Cep = dto.Cep!;
            enderecoExistente.Numero = dto.Numero!;
            enderecoExistente.Complemento = dto.Complemento!;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/Endereco/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEndereco(int id)
        {
            var Endereco = await _context.Enderecos
                .FirstOrDefaultAsync(c => c.Idendereco == id);

            if (Endereco == null)
            {
                return NotFound();
            }

            _context.Enderecos.Remove(Endereco);

            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool EnderecoExiste(int id)
        {
            return _context.Enderecos
                .Any(e => e.Idendereco == id);
        }
    }
}
