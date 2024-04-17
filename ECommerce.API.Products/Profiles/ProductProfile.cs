using AutoMapper;

namespace ECommerce.API.Products;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Db.Product, Models.Product>();
    }
}
