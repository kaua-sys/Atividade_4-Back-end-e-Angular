namespace Atividade_3.DTO
{
    public class PessoaDTO
    {
        public int IdPessoa { get; set; }

        public string? Nome { get; set; }

        public long? Cpf { get; set; }

        public DateOnly DataNascimento { get; set; }

        public string? Sexo { get; set;}

        public string? Email { get; set;}

        public string Telefone { get; set; } = null!;

        public string? AtivoPessoa { get; set; }

        public List<EnderecoDTO>? Enderecos { get; set;}
    }
}
