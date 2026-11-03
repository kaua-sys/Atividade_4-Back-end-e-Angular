using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Atividade_3.Models;

public partial class Endereco
{
    public int Idendereco { get; set; }

    public int Idpessoa { get; set; }

    public string Cep { get; set; } = null!;

    public string Numero { get; set; } = null!;

    public string Complemento { get; set; } = null!;

    [JsonIgnore]
    public virtual Pessoa IdpessoaNavigation { get; set; } = null!;


}
