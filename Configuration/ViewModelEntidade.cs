using ApiCart.Model;
using AutoMapper;

namespace ApiCart.Configuration
{
    public class ViewModelEntidade : Profile
    {
        public ViewModelEntidade() 
        {
            CreateMap<CarrinhoProduto, CarrinhoProdutoModel>()
                .ReverseMap();

            CreateMap<Produto, ProdutoModel>()
                .ReverseMap();

            CreateMap<Carrinho, CarrinhoModel>()
                .ReverseMap();
        }
    }
}
