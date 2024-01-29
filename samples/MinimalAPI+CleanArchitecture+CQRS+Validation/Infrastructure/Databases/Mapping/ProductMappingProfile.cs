using AutoMapper;

namespace Infrastructure.Databases.Mapping;

internal class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Entities.Product, Application.Products.Entities.Product>().ReverseMap();
        CreateMap<Entities.Product, Application.Brands.Entities.BrandProduct>().ReverseMap();
    }
}
