
namespace Atividade_3.DTO
{
    public class PessoaPostDTO
    {
        public string Nome { get; set; } = null!;
        public long Cpf { get; set; }
        public DateOnly DataNascimento { get; set; }
        public string Sexo { get; set; } = null!;
        public string Email { get; set; } = null!;
        public long Telefone { get; set; }
        public string AtivoPessoa { get; set; } = null!;
    }
}