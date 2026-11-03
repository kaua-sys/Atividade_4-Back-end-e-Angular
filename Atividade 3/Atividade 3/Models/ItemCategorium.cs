using System;
using System.Collections.Generic;

namespace Atividade_3.Models;

public partial class ItemCategorium
{
    public int Iditem { get; set; }

    public int Idcategoria { get; set; }

    public string Item { get; set; } = null!;

    public string DescricaoItem { get; set; } = null!;

    public decimal Preco { get; set; }

    public string TipoItem { get; set; } = null!;

    public string AtivoProdutoServico { get; set; } = null!;

    public string Categoria { get; set; } = null!;
}
