using Application.Common;
using Application.Common.Exceptions;
using Application.Products.Entities;


namespace Application.Products.Queries.GetProductById;

internal class GetProductByIdHandler(
    IProductRepository repository
) : IQueryHandler<GetProductByIdQuery, Product>
{
    public async ValueTask<QueryResult<Product>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductById(request.Id, cancellationToken);
        if (product is null)
        {
            throw new NotFoundException($"The product with ID='{request.Id}' not found.");
        }
        return product;
    }
}
