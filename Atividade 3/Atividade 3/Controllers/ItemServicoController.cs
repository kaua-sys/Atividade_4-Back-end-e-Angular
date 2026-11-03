
using Atividade_3.Models;
using Atividade_3.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atividade_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemServicoController : ControllerBase
    {
        private readonly BdAulaBackendContext _context;

        public ItemServicoController(BdAulaBackendContext context)
        {
            _context = context;
        }

        // GET: api/ItemServico
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetItens()
        {
            var itens = await _context.Items
                .Select(i => new ItemDTO
                {
                    Iditem = i.Iditem,
                    Item1 = i.Item1,
                    DescricaoItem = i.DescricaoItem,
                    Preco = i.Preco,
                    TipoItem = i.TipoItem,
                    AtivoProdutoServico = i.AtivoProdutoServico,
                    CategoriaIdcategoria = i.CategoriaIdcategoria
                })
                .ToListAsync();

            return Ok(itens);
        }

        // GET: api/ItemServico/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ItemDTO>> GetItem(int id)
        {
            var item = await _context.Items
                .Where(i => i.Iditem == id)
                .Select(i => new ItemDTO
                {
                    Iditem = i.Iditem,
                    Item1 = i.Item1,
                    DescricaoItem = i.DescricaoItem,
                    Preco = i.Preco,
                    TipoItem = i.TipoItem,
                    AtivoProdutoServico = i.AtivoProdutoServico,
                    CategoriaIdcategoria = i.CategoriaIdcategoria
                })
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return NotFound(new
                {
                    mensagem = "Item não encontrado."
                });
            }

            return Ok(item);
        }

        // POST: api/ItemServico
        [HttpPost]
        public async Task<ActionResult> PostItem(ItemPostDTO dados)
        {
            var categoriaExiste = await _context.Categoria
                .AnyAsync(c => c.Idcategoria == dados.CategoriaIdcategoria);

            if (!categoriaExiste)
            {
                return BadRequest(new
                {
                    mensagem = "Categoria não encontrada."
                });
            }

            var item = new Item
            {
                Item1 = dados.Item1,
                DescricaoItem = dados.DescricaoItem,
                Preco = dados.Preco,
                TipoItem = dados.TipoItem,
                AtivoProdutoServico = dados.AtivoProdutoServico,
                CategoriaIdcategoria = dados.CategoriaIdcategoria
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            var resultado = new ItemDTO
            {
                Iditem = item.Iditem,
                Item1 = item.Item1,
                DescricaoItem = item.DescricaoItem,
                Preco = item.Preco,
                TipoItem = item.TipoItem,
                AtivoProdutoServico = item.AtivoProdutoServico,
                CategoriaIdcategoria = item.CategoriaIdcategoria
            };

            return CreatedAtAction(
                nameof(GetItem),
                new { id = item.Iditem },
                resultado
            );
        }

        // PUT: api/ItemServico/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutItem(int id, ItemPostDTO dados)
        {
            var item = await _context.Items
                .FirstOrDefaultAsync(i => i.Iditem == id);

            if (item == null)
            {
                return NotFound(new
                {
                    mensagem = "Item não encontrado."
                });
            }

            var categoriaExiste = await _context.Categoria
                .AnyAsync(c => c.Idcategoria == dados.CategoriaIdcategoria);

            if (!categoriaExiste)
            {
                return BadRequest(new
                {
                    mensagem = "Categoria não encontrada."
                });
            }

            item.Item1 = dados.Item1;
            item.DescricaoItem = dados.DescricaoItem;
            item.Preco = dados.Preco;
            item.TipoItem = dados.TipoItem;
            item.AtivoProdutoServico = dados.AtivoProdutoServico;
            item.CategoriaIdcategoria = dados.CategoriaIdcategoria;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Item alterado com sucesso."
            });
        }

        // DELETE: api/ItemServico/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items
                .FirstOrDefaultAsync(i => i.Iditem == id);

            if (item == null)
            {
                return NotFound(new
                {
                    mensagem = "Item não encontrado."
                });
            }

            var possuiAtendimentos = await _context.Atendimentoitems
                .AnyAsync(a => a.Iditem == id);

            var possuiEstoque = await _context.Estoques
                .AnyAsync(e => e.Iditem == id);

            var possuiEntradas = await _context.EntradaProdutos
                .AnyAsync(e => e.Iditem == id);

            var possuiProduto = await _context.Produtos
                .AnyAsync(p => p.Iditem == id);

            if (possuiAtendimentos || possuiEstoque ||
                possuiEntradas || possuiProduto)
            {
                return BadRequest(new
                {
                    mensagem = "Não é possível excluir o item porque existem registros vinculados a ele."
                });
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Item excluído com sucesso."
            });
        }
    }
}
