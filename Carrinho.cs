using System.ComponentModel.DataAnnotations;

namespace ApiCart
{
    public class Carrinho
    {
        
        public int Id { get; set; }
        public List<CarrinhoProduto> CarrinhoProduto { get; set; } = new List<CarrinhoProduto>();

        public bool Finalizado { get; set; } = false;
    }
}
