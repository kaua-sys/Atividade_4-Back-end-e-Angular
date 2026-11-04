using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Atividade_3.Models;
using Atividade_3.DTO;

namespace Atividade_3.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class EstoqueController : ControllerBase
  {
    private readonly BdAulaBackendContext _context;

    public EstoqueController(BdAulaBackendContext context)
    {
      _context = context;
    }

    // Listar todos os produtos do estoque
    [HttpGet]
    public async Task<ActionResult> GetEstoque()
    {
      var estoque = await _context.Estoques
          .Include(e => e.IditemNavigation)
          .ThenInclude(p => p.IditemNavigation)
          .Where(e => e.IditemNavigation.IditemNavigation.TipoItem == "P")
          .Select(e => new
          {
            e.Iditem,
            Nome = e.IditemNavigation.IditemNavigation.Item1,
            Quantidade = e.QuantidadeEstoque
          })
          .ToListAsync();

      return Ok(estoque);
    }

    // Consultar quantidade de um produto específico
    [HttpGet("{id}")]
    public async Task<ActionResult> GetEstoquePorId(int id)
    {
      var estoque = await _context.Estoques
          .Include(e => e.IditemNavigation)
          .ThenInclude(p => p.IditemNavigation)
          .Where(e => e.Iditem == id &&
                      e.IditemNavigation.IditemNavigation.TipoItem == "P")
          .Select(e => new
          {
            e.Iditem,
            Nome = e.IditemNavigation.IditemNavigation.Item1,
            Quantidade = e.QuantidadeEstoque
          })
          .FirstOrDefaultAsync();

      if (estoque == null)
      {
        return NotFound(new
        {
          mensagem = "Produto não encontrado no estoque."
        });
      }

      return Ok(estoque);
    }

    [HttpPost]
    public async Task<ActionResult> PostEstoque(EstoquePostDTO dto)
    {
      var produto = await _context.Produtos
          .Include(p => p.IditemNavigation)
          .FirstOrDefaultAsync(p => p.Iditem == dto.Iditem);

      if (produto == null)
        return NotFound(new { mensagem = "Produto não encontrado." });

      if (produto.IditemNavigation.TipoItem != "P")
        return BadRequest(new { mensagem = "Só é permitido cadastrar produtos no estoque." });

      var estoque = await _context.Estoques
          .FirstOrDefaultAsync(e => e.Iditem == dto.Iditem);

      if (estoque == null)
      {
        estoque = new Estoque
        {
          Iditem = dto.Iditem,
          QuantidadeEstoque = dto.QuantidadeEstoque
        };

        _context.Estoques.Add(estoque);
      }
      else
      {
        estoque.QuantidadeEstoque += dto.QuantidadeEstoque;
      }

      await _context.SaveChangesAsync();

      return Ok(new
      {
        mensagem = "Estoque atualizado com sucesso!",
        quantidadeAtual = estoque.QuantidadeEstoque
      });
    }
  }
}
