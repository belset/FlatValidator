using Application.Brands;
using Application.Common;
using Application.Common.Exceptions;
using Application.Products.Entities;


namespace Application.Products.Commands.CreateProduct;

internal class CreateProductHandler(
    IProductRepository productRepository,
    IBrandRepository brandRepository
) : ICommandHandler<CreateProductCommand, Product>
{
    public async ValueTask<CommandResult<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var brand = await brandRepository.GetBrandByName(request.BrandName, cancellationToken);
        if (brand is null)
        {
            throw new NotFoundException($"Brand '{request.BrandName}' not found.");
        }

        if (await productRepository.ProductExists(request.ProductName, request.BrandName, cancellationToken))
        {
            throw new AlreadyExistsException($"The product '{request.ProductName}' for the brand '{request.BrandName}' already exists.");
        }

        return await productRepository.CreateProduct(request.ProductName, request.Rate, brand.Id, cancellationToken);
    }
}
