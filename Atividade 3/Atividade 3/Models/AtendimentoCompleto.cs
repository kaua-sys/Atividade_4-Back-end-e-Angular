using System;
using System.Collections.Generic;

namespace Atividade_3.Models;

public partial class AtendimentoCompleto
{
    public int Idatendimento { get; set; }

    public DateOnly DataAtedimento { get; set; }

    public string Cliente { get; set; } = null!;

    public string Produto { get; set; } = null!;

    public int QuantidadeItens { get; set; }

    public decimal ValorEntrada { get; set; }

    public int Idfuncionario { get; set; }

    public string Funcionario { get; set; } = null!;
}
