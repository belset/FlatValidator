using System.Validation;


namespace Application.Products.Commands.DeleteProduct;

internal class DeleteProductValidator : FlatValidator<DeleteProductCommand>
{
    public DeleteProductValidator(IProductRepository productRepository)
    {
        ErrorIf(x => x.Id.IsEmpty(), "Invalid Product ID.", x => x.Id);
        ValidIf(async x => await productRepository.ProductExists(x.Id), "Product not found.", x => x.Id);
    }
}
