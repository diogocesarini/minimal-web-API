using AutoMapper;

namespace ApiCart.Configuration
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration RegisterMappings() =>
            new MapperConfiguration(cfg =>
                cfg.AddProfile(new ViewModelEntidade()));
    }
}
