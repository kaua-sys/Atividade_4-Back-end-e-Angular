using System;
using System.Collections.Generic;

namespace Atividade_3.Models;

public partial class PessoaEndereco
{
    public string Nome { get; set; } = null!;

    public ulong Cpf { get; set; }

    public string Cep { get; set; } = null!;

    public string Numero { get; set; } = null!;

    public string Complemento { get; set; } = null!;
}
