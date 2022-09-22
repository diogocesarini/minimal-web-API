namespace ApiCart
{
    public class CarrinhoProduto
    {
        public int Id { get; set; }
        public int IdProduto { get; set; }
        public int IdCarrinho { get; set; }
        public int Quantidade { get; set; }

        public virtual Produto Produto { get; set; } = new Produto();
        public virtual Carrinho Carrinho { get; set; } = new Carrinho();
    }
}
