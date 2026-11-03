namespace Atividade_3.DTO
{
    public class FuncionarioDTO
    {
        public int IdPessoa { get; set; }

        public string? Cargo { get; set; }

        public DateOnly DataAdmissao { get; set; }

        public PessoaDTO? Pessoa { get; set; }
    }
}
