namespace Atividade_3.DTO
{
    public class ItemPostDTO
    {
        public string Item1 { get; set; } = null!;
        public string DescricaoItem { get; set; } = null!;
        public decimal Preco { get; set; }
        public string TipoItem { get; set; } = null!;
        public string AtivoProdutoServico { get; set; } = null!;
        public int CategoriaIdcategoria { get; set; }
    }
}
