using Application.Common;
using Application.Stores.Entities;


namespace Application.Stores.Queries.GetStores;

public class GetStoresHandler(
    IStoreRepository repository
) : IQueryHandler<GetStoresQuery, List<Store>>
{
    public async ValueTask<QueryResult<List<Store>>> Handle(GetStoresQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetStores(cancellationToken);
    }
}
