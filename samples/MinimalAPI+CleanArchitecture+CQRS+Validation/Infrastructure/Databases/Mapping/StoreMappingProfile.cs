using AutoMapper;

namespace Infrastructure.Databases.Mapping;

internal class StoreMappingProfile : Profile
{
    public StoreMappingProfile()
    {
        CreateMap<Entities.Store, Application.Stores.Entities.Store>().ReverseMap();
    }
}