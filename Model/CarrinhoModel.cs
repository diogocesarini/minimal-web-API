namespace ApiCart.Model
{
    public class CarrinhoModel
    {
        public CarrinhoModel()
        {
            Finalizado = false;
        }

        public int Id { get; set; }
        public List<CarrinhoProdutoModel> CarrinhoProduto { get; set; } = new List<CarrinhoProdutoModel>();

        public bool Finalizado { get; set; }
    }
}
