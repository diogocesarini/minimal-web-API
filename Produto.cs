using System.ComponentModel.DataAnnotations;

namespace ApiCart
{
    public class Produto
    {
       
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Nome { get; set; } = string.Empty;   
        public decimal ValorUnitario { get; set; } = decimal.Zero;
        public int Quantidade { get; set; }
    }
}
