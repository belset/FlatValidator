using System.Validation;


namespace Application.Brands.Queries.GetBrandById;

internal class GetBrandByIdValidator : FlatValidator<GetBrandByIdQuery>
{
    public GetBrandByIdValidator(IBrandRepository brandRepository)
    {
        ErrorIf(x => x.Id.IsEmpty(), "Brand ID cannot be empty.", x => x.Id);
        ValidIf(async x => await brandRepository.BrandExists(x.Id), "Brand not found.", x => x.Id);
    }
}
