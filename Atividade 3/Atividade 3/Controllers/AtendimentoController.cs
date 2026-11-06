using Microsoft.EntityFrameworkCore;
using Atividade_3.DTO;
using Atividade_3.Models;
using Microsoft.AspNetCore.Mvc;


namespace Atividade_3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtendimentoController : ControllerBase
    {   
        //Definindo atributo do Context
        private readonly BdAulaBackendContext _context;
         
        //Método Construtot
        public AtendimentoController(BdAulaBackendContext context)
        {
            _context = context;
        }

        //Listar Atendimentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AtendimentoListDTO>>> GetAll()
        {
            var atendimentos = await _context.Atendimentos.Select(a => new AtendimentoListDTO
            {
                IdAtendimento = a.Idatendimento,
                IdPessoa = a.Idpessoa,
                NomePessoa = a.IdpessoaNavigation.Nome,
                DataAtendimento = a.DataAtedimento,
                StatusAtendimento = a.StatusAtedimento,

                Itens =  a.Atendimentoitems.Select(i => new AtendimentoItemListDTO
                {
                    IdAtendimento = i.Idatendimento,
                    IdItem = i.Iditem,
                    Item = i.IditemNavigation.Item1,
                    IdFuncionario = i.Idfuncionario,
                    NomeFuncionario = i.IdfuncionarioNavigation.IdpessoaNavigation.Nome,
                    Cargo = i.IdfuncionarioNavigation.Cargo,
                    Quantidade = i.QuantidadeItens,
                    ValorUnitario = i.ValorunitarioEntrada,
                }).ToList()
            }).ToListAsync();

              return Ok(atendimentos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AtendimentoListDTO>> GetAtendimento(int id)
        {
            var atendimento = await _context.Atendimentos.Where(a => a.Idatendimento == id).Select(a => new AtendimentoListDTO
            {
                IdAtendimento = a.Idatendimento,
                IdPessoa = a.Idpessoa,
                NomePessoa = a.IdpessoaNavigation.Nome,
                DataAtendimento = a.DataAtedimento,
                StatusAtendimento = a.StatusAtedimento,

                Itens = a.Atendimentoitems.Select(i =>  new AtendimentoItemListDTO
                {
                    IdAtendimento = i.Idatendimento,
                    IdItem = i.Iditem,
                    Item = i.IditemNavigation.Item1,
                    IdFuncionario = i.Idfuncionario,
                    NomeFuncionario = i.IdfuncionarioNavigation.IdpessoaNavigation.Nome,
                    Cargo = i.IdfuncionarioNavigation.Cargo,
                    Quantidade = i.QuantidadeItens,
                    ValorUnitario = i.ValorunitarioEntrada,
                }).ToList()
            }).FirstOrDefaultAsync();

            if(atendimento == null)
            {
                return NotFound(new
                {
                    memsagem = "Atendimento não Localizado"
                });
            }

            return Ok(atendimento);
        }

        // Listar atendimentos de um cliente
        [HttpGet("cliente/{idPessoa:int}")]
        public async Task<ActionResult<IEnumerable<AtendimentoListDTO>>> GetAtendimentosCliente(int idPessoa)
        {
            var atendimentos = await _context.Atendimentos
                .Where(a => a.Idpessoa == idPessoa)
                .Select(a => new AtendimentoListDTO
                {
                    IdAtendimento = a.Idatendimento,
                    IdPessoa = a.Idpessoa,
                    NomePessoa = a.IdpessoaNavigation.Nome,
                    DataAtendimento = a.DataAtedimento,
                    StatusAtendimento = a.StatusAtedimento,

                    Itens = a.Atendimentoitems.Select(i => new AtendimentoItemListDTO
                    {
                        IdAtendimento = i.Idatendimento,
                        IdItem = i.Iditem,
                        Item = i.IditemNavigation.Item1,
                        IdFuncionario = i.Idfuncionario,
                        NomeFuncionario = i.IdfuncionarioNavigation.IdpessoaNavigation.Nome,
                        Cargo = i.IdfuncionarioNavigation.Cargo,
                        Quantidade = i.QuantidadeItens,
                        ValorUnitario = i.ValorunitarioEntrada
                    }).ToList()
                })
                .ToListAsync();

            return Ok(atendimentos);
        }

    [HttpGet("status/{status}")]
    public async Task<ActionResult> GetPorStatus(string status)
    {
      var atendimentos = await _context.Atendimentos
          .Where(a => a.StatusAtedimento == status.ToUpper())
          .Select(a => new AtendimentoListDTO
          {
            IdAtendimento = a.Idatendimento,
            IdPessoa = a.Idpessoa,
            NomePessoa = a.IdpessoaNavigation.Nome,
            DataAtendimento = a.DataAtedimento,
            StatusAtendimento = a.StatusAtedimento,

            Itens = a.Atendimentoitems.Select(i => new AtendimentoItemListDTO
            {
              IdAtendimento = i.Idatendimento,
              IdItem = i.Iditem,
              Item = i.IditemNavigation.Item1,
              IdFuncionario = i.Idfuncionario,
              NomeFuncionario = i.IdfuncionarioNavigation.IdpessoaNavigation.Nome,
              Cargo = i.IdfuncionarioNavigation.Cargo,
              Quantidade = i.QuantidadeItens,
              ValorUnitario = i.ValorunitarioEntrada
            }).ToList()
          })
          .ToListAsync();

      return Ok(atendimentos);
    }

    [HttpPost]
        public async Task<ActionResult> PostAtendimento(AtendimentoPostDTO dto)
        {
            if (dto == null || dto.Itens == null || !dto.Itens.Any())
            {
                return BadRequest(new { mensagem = "Informe os dados e pelo menos um item." });
            }

            // Verifica se a pessoa existe
            var pessoaExiste = await _context.Pessoas
                .AnyAsync(p => p.Idpessoa == dto.IdPessoa);

            if (!pessoaExiste)
            {
                return BadRequest(new { mensagem = "Pessoa não encontrada." });
            }

           // Busca o funcionário escolhido pelo cliente
           var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(f => f.Idpessoa == dto.IdFuncionario &&f.IdpessoaNavigation.AtivoPessoa == "A");

          if (funcionario == null){
           return BadRequest(new { mensagem = "Funcionário não encontrado ou inativo." });
           }
           var atendimento = new Atendimento
              {
                Idpessoa = dto.IdPessoa,
                DataAtedimento = DateOnly.FromDateTime(DateTime.Today),
                StatusAtedimento = dto.StatusAtendimento,

                Atendimentoitems = new List<Atendimentoitem>()
              };

            foreach (var itemDto in dto.Itens)
            {
                if (itemDto.Quantidade <= 0)
                {
                    return BadRequest(new { mensagem = "A quantidade deve ser maior que zero." });
                }

                // Busca o item e seu preço
                var item = await _context.Items
                    .FirstOrDefaultAsync(i => i.Iditem == itemDto.IdItem);

                if (item == null)
                {
                    return BadRequest(new { mensagem = $"Item {itemDto.IdItem} não encontrado." });
                }

                atendimento.Atendimentoitems.Add(new Atendimentoitem
                {
                    Iditem = itemDto.IdItem,
                    Idfuncionario = funcionario.Idpessoa,
                    QuantidadeItens = itemDto.Quantidade,
                    ValorunitarioEntrada = item.Preco
                });
            }

              await using var transaction = await _context.Database.BeginTransactionAsync();

              try
              {
              _context.Atendimentos.Add(atendimento);

               await _context.SaveChangesAsync();

              await transaction.CommitAsync();

              return Ok(new
              {
               mensagem = "Atendimento cadastrado com sucesso."
             });
             }
             catch (Exception)
             {   
             await transaction.RollbackAsync();

             return BadRequest(new
             {
             mensagem = "Erro ao cadastrar. Verifique o estoque disponível."
          });
      }
    }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutAtendimento(int id, AtendimentoPutDTO dto)
        {
            if (dto == null || dto.Itens == null)
            {
                return BadRequest(new { mensagem = "Dados inválidos." });
            }

            var atendimento = await _context.Atendimentos
                .Include(a => a.Atendimentoitems)
                .FirstOrDefaultAsync(a => a.Idatendimento == id);

            if (atendimento == null)
            {
                return NotFound(new { mensagem = "Atendimento não localizado." });
            }

            atendimento.StatusAtedimento = dto.StatusAtendimento;

            var itensRecebidos = dto.Itens
                .Select(i => (i.IdItem, i.IdFuncionario))
                .ToHashSet();

            var itensRemover = atendimento.Atendimentoitems
                .Where(i => !itensRecebidos.Contains((i.Iditem, i.Idfuncionario)))
                .ToList();

            _context.Atendimentoitems.RemoveRange(itensRemover);

            foreach (var itemDto in dto.Itens)
            {
                var itemExistente = atendimento.Atendimentoitems
                    .FirstOrDefault(i =>
                        i.Iditem == itemDto.IdItem &&
                        i.Idfuncionario == itemDto.IdFuncionario);

                if (itemExistente != null)
                {
                    itemExistente.QuantidadeItens = itemDto.Quantidade;
                    itemExistente.ValorunitarioEntrada = itemDto.ValorUnitario;
                }
                else
                {
                    atendimento.Atendimentoitems.Add(new Atendimentoitem
                    {
                        Idatendimento = atendimento.Idatendimento,
                        Iditem = itemDto.IdItem,
                        Idfuncionario = itemDto.IdFuncionario,
                        QuantidadeItens = itemDto.Quantidade,
                        ValorunitarioEntrada = itemDto.ValorUnitario
                    });
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
    }

        // Alterar situação do pedido
        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> PutStatus(int id, AtendimentoStatusDTO dto)
    {
        var atendimento = await _context.Atendimentos
          .FirstOrDefaultAsync(a => a.Idatendimento == id);

        if (atendimento == null)
        {
        return NotFound(new { mensagem = "Pedido não encontrado." });
        }

        var status = dto.StatusAtendimento.ToUpper();

         if (status != "A" && status != "P" &&
          status != "C" && status != "X")
         {
         return BadRequest(new { mensagem = "Situação inválida." });
         }

        // Não permite alterar pedidos concluídos ou cancelados
        if (atendimento.StatusAtedimento == "C" ||
          atendimento.StatusAtedimento == "X")
        {
        return BadRequest(new
        {
          mensagem = "Não é possível alterar um pedido concluído ou cancelado."
        });
      }

      atendimento.StatusAtedimento = status;

      await _context.SaveChangesAsync();

      return Ok(new
      {
        mensagem = "Situação do pedido alterada com sucesso."
      });
    }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAtendimento(int id)
        {
            var atendimento = await _context.Atendimentos
                .Include(a => a.Atendimentoitems)
                .FirstOrDefaultAsync(a => a.Idatendimento == id);

            if (atendimento == null)
            {
                return NotFound(new
                {
                    mensagem = "Atendimento não localizado."
                });
            }

            _context.Atendimentoitems.RemoveRange(atendimento.Atendimentoitems);
            _context.Atendimentos.Remove(atendimento);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
