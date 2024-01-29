using Application.Brands;
using Application.Common;
using Application.Common.Exceptions;

namespace Application.Products.Commands.UpdateProduct;

internal class UpdateProductHandler(
    IProductRepository productRepository,
    IBrandRepository brandRepository
) : ICommandHandler<UpdateProductCommand, bool>
{
    public async ValueTask<CommandResult<bool>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetProductById(request.Id, cancellationToken);
        if (product is null)
        {
            throw new NotFoundException($"The product with ID='{request.Id}' not found.");
        }

        var anotherProduct = await productRepository.GetProductByName(request.ProductName, request.BrandName, cancellationToken);
        if (anotherProduct is not null && anotherProduct.Id != product.Id)
        {
            var brand = await brandRepository.GetBrandById(anotherProduct.BrandId, cancellationToken);
            throw new AlreadyExistsException($"The product '{anotherProduct.ProductName}' for the brand '{brand.BrandName}' already exists.");
        }

        return await productRepository.UpdateProduct(request.Id, request.ProductName, request.Rate, cancellationToken);
    }
}
