using Application.Brands.Entities;
using Application.Common;


namespace Application.Brands.Queries.GetBrands;

public class GetBrandsHandler(
    IBrandRepository repository
) : IQueryHandler<GetBrandsQuery, List<Brand>>
{
    public async ValueTask<QueryResult<List<Brand>>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetBrands(cancellationToken);
    }
}
