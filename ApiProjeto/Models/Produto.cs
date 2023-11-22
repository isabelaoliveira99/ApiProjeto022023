namespace ApiProjeto.Models
{
    public class Produto
    {
        public int? id { get; set; }
        public string? nome_produto { get; set; }
        public string? descricao_produto { get; set; }
        public decimal? valor { get; set; }
        public int? id_categoria { get; set; }
    }
}
