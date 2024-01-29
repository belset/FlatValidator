using Application.Common;
using Application.Products.Entities;


namespace Application.Products.Queries.GetProducts;

public class GetProductsHandler(
    IProductRepository repository
) : IQueryHandler<GetProductsQuery, List<Product>>
{
    public async ValueTask<QueryResult<List<Product>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetProducts(cancellationToken);
    }
}
