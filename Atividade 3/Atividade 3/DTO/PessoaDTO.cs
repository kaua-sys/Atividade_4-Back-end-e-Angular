namespace Atividade_3.DTO
{
    public class PessoaDTO
    {
        public int IdPessoa { get; set; }

        public string? Nome { get; set; }

        public ulong? Cpf { get; set; }

        public DateOnly DataNascimento { get; set; }

        public string? Sexo { get; set;}

        public string? Email { get; set;}

        public long? Telefone { get; set; }

        public string? AtivoPessoa { get; set; }

        public List<EnderecoDTO>? Enderecos { get; set;}
    }
}
