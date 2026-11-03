using System;
using System.Collections.Generic;

namespace Atividade_3.Models;

public partial class FuncionarioEndereco
{
    public int Idpessoa { get; set; }

    public string Cargo { get; set; } = null!;

    public DateOnly DataAdmissao { get; set; }
}
