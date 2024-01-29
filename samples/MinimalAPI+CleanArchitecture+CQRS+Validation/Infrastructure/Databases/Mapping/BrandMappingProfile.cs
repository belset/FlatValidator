using AutoMapper;


namespace Infrastructure.Databases.Mapping;

internal class BrandMappingProfile : Profile
{
    public BrandMappingProfile()
    {
        CreateMap<Entities.Brand, Application.Brands.Entities.Brand>().ReverseMap();
    }
}
