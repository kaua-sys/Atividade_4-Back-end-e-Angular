
namespace Atividade_3.DTO
{
    public class ItemDTO
    {
        public int Iditem { get; set; }
        public string Item1 { get; set; } = null!;
        public string DescricaoItem { get; set; } = null!;
        public decimal Preco { get; set; }
        public string TipoItem { get; set; } = null!;
        public string AtivoProdutoServico { get; set; } = null!;
        public int CategoriaIdcategoria { get; set; }
    }
}
