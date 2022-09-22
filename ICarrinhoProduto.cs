namespace ApiCart
{
    public interface ICarrinhoProduto
    {
        Carrinho Carrinho { get; set; }
        int Id { get; set; }
        int IdCarrinho { get; set; }
        int IdProduto { get; set; }
        Produto Produto { get; set; }
        int Quantidade { get; set; }
    }
}