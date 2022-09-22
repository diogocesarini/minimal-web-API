namespace ApiCart.Model
{
    public class ProdutoModel
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal ValorUnitario { get; set; } = decimal.Zero;
        public int Quantidade { get; set; }
    }
}
