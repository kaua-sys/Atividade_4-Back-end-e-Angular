namespace Atividade_3.DTO
{
    public class ProdutoDTO
    {
        //Dados da tabela Item
        public int IdCategoria { get; set; }

        public string? Item { get; set; }

        public string? DescricaoItem { get; set; }

        public decimal Preco { get; set; }

        public string? TipoItem { get; set; }

        public string? AtivoProdutoServico { get; set; }

        //Dados da tabela Produto
        public int CodigoPreco { get; set; }

        public decimal PrecoVenda { get; set; }

        public string? Unidade { get; set; }
    }
}
