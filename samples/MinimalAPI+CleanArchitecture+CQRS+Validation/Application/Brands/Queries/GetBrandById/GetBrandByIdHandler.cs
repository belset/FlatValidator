using Application.Brands.Entities;
using Application.Common;
using Application.Common.Exceptions;


namespace Application.Brands.Queries.GetBrandById;

public class GetBrandByIdHandler(IBrandRepository repository) : IQueryHandler<GetBrandByIdQuery, Brand>
{
    public async ValueTask<QueryResult<Brand>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
    {
        var brand = await repository.GetBrandById(request.Id, cancellationToken);
        if (brand is null)
        {
            return new NotFoundException($"Brand with ID:'{request.Id}' not found.");
        }
        return brand;
    }
}
