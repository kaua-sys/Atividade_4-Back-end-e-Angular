using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Atividade_3.Models;

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
  }
}
