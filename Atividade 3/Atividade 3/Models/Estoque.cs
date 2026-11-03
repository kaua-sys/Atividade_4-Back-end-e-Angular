using System;
using System.Collections.Generic;

namespace Atividade_3.Models;

public partial class Estoque
{
    public int Iditem { get; set; }

    public uint QuantidadeEstoque { get; set; }

    public virtual ICollection<EntradaProduto> EntradaProdutos { get; set; } = new List<EntradaProduto>();

    public virtual Produto IditemNavigation { get; set; } = null!;
}
