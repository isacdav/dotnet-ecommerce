using AutoMapper;

namespace ECommerce.API.Customers.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Db.Customer, Models.Customer>();
    }
}
