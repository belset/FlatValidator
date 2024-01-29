using System.Validation;


namespace Application.Stores.Queries.GetStoreById;

internal class GetStoreByIdValidator : FlatValidator<GetStoreByIdQuery>
{
    public GetStoreByIdValidator(IStoreRepository storeRepository)
    {
        ErrorIf(x => x.Id.IsEmpty(), errorMessage: "Store ID cannot be empty.", x => x.Id);
        ValidIf(async x => await storeRepository.StoreExists(x.Id), errorMessage: "Store not found.", x => x.Id);
    }
}
