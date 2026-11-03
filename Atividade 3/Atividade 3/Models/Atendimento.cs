using System;
using System.Collections.Generic;

namespace Atividade_3.Models;

public partial class Atendimento
{
    public int Idatendimento { get; set; }

    public DateOnly DataAtedimento { get; set; }

    public string StatusAtedimento { get; set; } = null!;

    public int Idpessoa { get; set; }

    public virtual ICollection<Atendimentoitem> Atendimentoitems { get; set; } = new List<Atendimentoitem>();

    public virtual Pessoa IdpessoaNavigation { get; set; } = null!;
}
