using System;
using System.Collections.Generic;

namespace Atividade_3.Models;

public partial class ItemCategoriaEstoque
{
    public string Item { get; set; } = null!;

    public string Categoria { get; set; } = null!;

    public uint QuantidadeEstoque { get; set; }
}
