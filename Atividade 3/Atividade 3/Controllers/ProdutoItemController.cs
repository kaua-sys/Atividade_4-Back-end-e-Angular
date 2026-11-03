using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Atividade_3.DTO;
using Atividade_3.Models;

namespace Atividade_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoItemController : ControllerBase
    {
        //Declaração do atributo _context
        private readonly BdAulaBackendContext _context;

        //Declaração do método contrutor
        public ProdutoItemController(BdAulaBackendContext context)
        {
            //Instância o atributo _context
            _context = context;
        }

        //listar todos os produtos
        [HttpGet]
        public async Task<ActionResult> GetProdutos()
        {
            var produtos = await _context.Produtos
                .Include(p => p.IditemNavigation)
                .Select(p => new
                {
                    p.Iditem,
                    p.CodigoPreco,
                    p.PrecoVenda,
                    p.Unidade,

                    Item = new
                    {
                        p.IditemNavigation.Item1,
                        p.IditemNavigation.DescricaoItem,
                        p.IditemNavigation.Preco,
                        p.IditemNavigation.TipoItem,
                        p.IditemNavigation.AtivoProdutoServico,
                        p.IditemNavigation.CategoriaIdcategoria
                    }
                })
                .ToListAsync();

            return Ok(produtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetProduto(int id)
        {
            var produto = await _context.Produtos
                .Where(p => p.Iditem == id)
                .Select(p => new
                {
                    p.Iditem,
                    p.CodigoPreco,
                    p.PrecoVenda,
                    p.Unidade,

                    Item = new
                    {
                        p.IditemNavigation.Item1,
                        p.IditemNavigation.DescricaoItem,
                        p.IditemNavigation.Preco,
                        p.IditemNavigation.TipoItem,
                        p.IditemNavigation.AtivoProdutoServico,
                        p.IditemNavigation.CategoriaIdcategoria
                    }
                })
                .FirstOrDefaultAsync();

            if (produto == null)
            {
                return NotFound(new
                {
                    mensagem = "Produto não encontrado."
                });
            }

            return Ok(produto);
        }
        [HttpPost]

        public async Task<ActionResult> PostProduto(ProdutoDTO dados)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var categoriaExiste = await _context.Categoria
           .AnyAsync(c => c.Idcategoria == dados.IdCategoria);

            if (!categoriaExiste)
            {
                return BadRequest(new
                {
                    mensagem = "Categoria não encontrada."
                });
            }

            try
            {
                var item = new Item
                {
                    CategoriaIdcategoria = dados.IdCategoria,
                    Item1 = dados.Item,
                    DescricaoItem = dados.DescricaoItem,
                    Preco = dados.Preco,
                    TipoItem = dados.TipoItem,
                    AtivoProdutoServico = dados.AtivoProdutoServico
                };

                _context.Items.Add(item);

                //Salva o Item no banco
                await _context.SaveChangesAsync();

                int idItemGerado = item.Iditem;

                var produto = new Produto
                {
                    Iditem = idItemGerado,
                    CodigoPreco = dados.CodigoPreco,
                    PrecoVenda = dados.PrecoVenda,
                    Unidade = dados.Unidade
                };

                _context.Produtos.Add(produto);

                //Salva o Produto

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return CreatedAtAction(
                   nameof(GetProduto),
                   new { id = produto.Iditem },
                   produto
                   );
            }
            catch (Exception ex)
            {
                //Se alguma operação falhar
                // Se desfaz o Insert do Item e do Produto

                await transaction.RollbackAsync();

                return StatusCode(500, new
                {
                    message = "Erro ao cadastrar produto.",
                    erro = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, ProdutoDTO dados)
        {
            //Busca o produto junto com o item
            var produtoExistente = await _context.Produtos.Include(p => p.IditemNavigation).FirstOrDefaultAsync(p => p.Iditem == id);

            var categoriaExiste = await _context.Categoria
           .AnyAsync(c => c.Idcategoria == dados.IdCategoria);

            if (!categoriaExiste)
            {
                return BadRequest(new
                {
                    mensagem = "Categoria não encontrada."
                });
            }


            if (produtoExistente == null)
            {
                return NotFound(new
                {
                    mensagem = "Produto não encontrado."
                });
            }

            produtoExistente.CodigoPreco = dados.CodigoPreco;
            produtoExistente.PrecoVenda = dados.PrecoVenda;
            produtoExistente.Unidade = dados.Unidade;

            produtoExistente.IditemNavigation.CategoriaIdcategoria = dados.IdCategoria;
            produtoExistente.IditemNavigation.Item1 = dados.Item;
            produtoExistente.IditemNavigation.DescricaoItem = dados.DescricaoItem;
            produtoExistente.IditemNavigation.Preco = dados.Preco;
            produtoExistente.IditemNavigation.TipoItem = dados.TipoItem;
            produtoExistente.IditemNavigation.AtivoProdutoServico = dados.AtivoProdutoServico;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound(new
                    {
                        mensagem = "Produto não encontrado."
                    });
                }
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteProduto(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var possuiAtendimentos = await _context.Atendimentoitems
           .AnyAsync(a => a.Iditem == id);

            var possuiEstoque = await _context.Estoques
                .AnyAsync(e => e.Iditem == id);

            var possuiEntradas = await _context.EntradaProdutos
                .AnyAsync(e => e.Iditem == id);

            if (possuiAtendimentos || possuiEstoque || possuiEntradas)
            {
                return BadRequest(new
                {
                    mensagem = "Não é possível excluir o produto porque existem registros vinculados a ele."
                });
            }

            try
            {
                var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Iditem == id);

                if (produto == null)
                {
                    return NotFound(new
                    {
                        mensagem = "Produto não encontrado"
                    });
                }
                //Remove o Produto
                _context.Produtos.Remove(produto);

                await _context.SaveChangesAsync();

                var item = await _context.Items.FirstOrDefaultAsync(i => i.Iditem == id);

                if (item != null)
                {
                    _context.Items.Remove(item);
                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return StatusCode(500, new
                {
                    mensagem = "Erro ao exclur produto",
                    erro = ex.Message
                });
            }
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(p => p.Iditem == id);
        }
    }
}
