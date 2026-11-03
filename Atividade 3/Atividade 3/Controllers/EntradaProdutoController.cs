using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Atividade_3.DTO;
using Atividade_3.Controllers;
using Atividade_3.Models;


namespace Atividade_3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntradaProdutoController : ControllerBase
    {
        //Conexão com o banco de dados
        private readonly BdAulaBackendContext _context;

        public EntradaProdutoController(BdAulaBackendContext context)
        {
            _context = context;
        }

        //Lista todas entradas de produtos Com data,unidade e seu valor unitario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntradaProduto>>> GetEntradaProduto()
        {
            var entradas = await _context.EntradaProdutos.Select(e => new EntradaProduto
            {
                IdentradaProduto = e.IdentradaProduto,
                Iditem = e.Iditem,
                QuantidadeEntrada = e.QuantidadeEntrada,
                DataEntradaProduto = e.DataEntradaProduto,
                ValorUnitarioEntrada = e.ValorUnitarioEntrada,
                Unidade = e.Unidade,
            }).ToListAsync();
            return Ok(entradas);
        }

        //Busca um funcionário pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<EntradaProdutoDTO>> GetEntradaProduto(int id)
        {
            var entrada = await _context.EntradaProdutos.Where(p => p.IdentradaProduto == id).Select(e => new EntradaProdutoDTO
            {
                IdentradaProduto = e.IdentradaProduto,
                Iditem = e.Iditem,
                Unidade = e.Unidade,
                DataEntradaProduto = e.DataEntradaProduto,
                QuantidadeEntrada = e.QuantidadeEntrada,
                ValorUnitarioEntrada = e.ValorUnitarioEntrada,
            }).FirstOrDefaultAsync();

            //Verifica se existe uma entrada
            if (entrada == null)
            {
                return NotFound("Essa entrada não foi encontrada por favor verifique novamente");
            }

            return Ok(entrada);
        }

        [HttpPost]
        public async Task<ActionResult<EntradaProdutoDTO>> Post(EntradaProdutoDTO dto)
        {
            // Verifica se os dados foram informados
            if (dto == null)
            {
                return BadRequest("Informe os dados da entrada.");
            }

            // Verifica se existe estoque para esse item
            var estoque = await _context.Estoques
                .FirstOrDefaultAsync(e => e.Iditem == dto.Iditem);

            if (estoque == null)
            {
                return BadRequest("Não existe estoque cadastrado para esse item.");
            }

            // Cria a entidade
            var entrada = new EntradaProduto
            {
                Iditem = dto.Iditem,
                DataEntradaProduto = dto.DataEntradaProduto,
                QuantidadeEntrada = dto.QuantidadeEntrada,
                Unidade = dto.Unidade,
                ValorUnitarioEntrada = dto.ValorUnitarioEntrada
            };

            // Adiciona a entrada
            _context.EntradaProdutos.Add(entrada);

            // Atualiza a quantidade do estoque
            estoque.QuantidadeEstoque += (uint)dto.QuantidadeEntrada;

            await _context.SaveChangesAsync();

            // Retorna os dados cadastrados
            dto.IdentradaProduto = entrada.IdentradaProduto;

            return CreatedAtAction(
                nameof(GetEntradaProduto),
                new { id = entrada.IdentradaProduto },
                dto
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EntradaProdutoDTO>> Put(
    int id, EntradaProdutoDTO dto)
        {
            // Busca a entrada
            var entrada = await _context.EntradaProdutos
                .FirstOrDefaultAsync(e => e.IdentradaProduto == id);

            // Verifica se a entrada existe
            if (entrada == null)
            {
                return NotFound("Entrada não encontrada.");
            }

            // Verifica se a quantidade é válida
            if (dto.QuantidadeEntrada <= 0 ||
                dto.QuantidadeEntrada != decimal.Truncate(dto.QuantidadeEntrada) ||
                dto.QuantidadeEntrada > uint.MaxValue)
            {
                return BadRequest("Informe uma quantidade inteira positiva.");
            }

            // Busca o estoque antigo
            var estoqueAntigo = await _context.Estoques
                .FirstOrDefaultAsync(e => e.Iditem == entrada.Iditem);

            if (estoqueAntigo == null)
            {
                return BadRequest("Estoque antigo não encontrado.");
            }

            // Busca o estoque do item informado
            var estoqueNovo = await _context.Estoques
                .FirstOrDefaultAsync(e => e.Iditem == dto.Iditem);

            if (estoqueNovo == null)
            {
                return BadRequest("Estoque do novo item não encontrado.");
            }

            uint quantidadeAntiga = (uint)entrada.QuantidadeEntrada;
            uint quantidadeNova = (uint)dto.QuantidadeEntrada;

            // Atualiza as quantidades do estoque
            if (estoqueAntigo.Iditem == estoqueNovo.Iditem)
            {
                if (quantidadeNova > quantidadeAntiga)
                {
                    estoqueAntigo.QuantidadeEstoque +=
                        quantidadeNova - quantidadeAntiga;
                }
                else
                {
                    uint diferenca = quantidadeAntiga - quantidadeNova;

                    if (estoqueAntigo.QuantidadeEstoque < diferenca)
                    {
                        return BadRequest(
                            "Quantidade insuficiente no estoque para essa alteração.");
                    }

                    estoqueAntigo.QuantidadeEstoque -= diferenca;
                }
            }
            else
            {
                if (estoqueAntigo.QuantidadeEstoque < quantidadeAntiga)
                {
                    return BadRequest(
                        "Quantidade insuficiente no estoque antigo.");
                }

                estoqueAntigo.QuantidadeEstoque -= quantidadeAntiga;
                estoqueNovo.QuantidadeEstoque += quantidadeNova;
            }

            // Atualiza os dados da entrada
            entrada.Iditem = dto.Iditem;
            entrada.DataEntradaProduto = dto.DataEntradaProduto;
            entrada.QuantidadeEntrada = dto.QuantidadeEntrada;
            entrada.Unidade = dto.Unidade;
            entrada.ValorUnitarioEntrada = dto.ValorUnitarioEntrada;

            // Salva as alterações
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            // Busca a entrada
            var entrada = await _context.EntradaProdutos
                .FirstOrDefaultAsync(e => e.IdentradaProduto == id);

            // Verifica se a entrada existe
            if (entrada == null)
            {
                return NotFound("Essa entrada não foi encontrada.");
            }

            // Busca o estoque relacionado
            var estoque = await _context.Estoques
                .FirstOrDefaultAsync(e => e.Iditem == entrada.Iditem);

            // Verifica se existe estoque
            if (estoque == null)
            {
                return BadRequest("Estoque não encontrado para esse item.");
            }

            // Verifica se há quantidade suficiente no estoque
            if (entrada.QuantidadeEntrada <= 0 ||
                entrada.QuantidadeEntrada != decimal.Truncate(entrada.QuantidadeEntrada) ||
                entrada.QuantidadeEntrada > uint.MaxValue)
            {
                return BadRequest("A quantidade registrada na entrada é inválida.");
            }

            uint quantidade = (uint)entrada.QuantidadeEntrada;

            if (estoque.QuantidadeEstoque < quantidade)
            {
                return BadRequest(
                    "Não é possível excluir essa entrada, pois a quantidade atual do estoque é insuficiente.");
            }

            // Desconta a quantidade do estoque
            estoque.QuantidadeEstoque -= quantidade;

            // Remove a entrada
            _context.EntradaProdutos.Remove(entrada);

            // Salva as alterações
            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
