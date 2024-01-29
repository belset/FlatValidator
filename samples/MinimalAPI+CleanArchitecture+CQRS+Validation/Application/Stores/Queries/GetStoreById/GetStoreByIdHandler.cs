using Application.Common;
using Application.Stores.Entities;


namespace Application.Stores.Queries.GetStoreById;

public class GetStoreByIdHandler(
    IStoreRepository repository
) : IQueryHandler<GetStoreByIdQuery, Store>
{
    public async ValueTask<QueryResult<Store>> Handle(GetStoreByIdQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetStoreById(request.Id, cancellationToken);
    }
}
