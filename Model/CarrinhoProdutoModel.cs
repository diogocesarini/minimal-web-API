namespace ApiCart.Model
{
    public class CarrinhoProdutoModel
    {
        public int Id { get; set; }
        public int IdProduto { get; set; }
        public int IdCarrinho { get; set; }
        public int Quantidade { get; set; }
    }
}
