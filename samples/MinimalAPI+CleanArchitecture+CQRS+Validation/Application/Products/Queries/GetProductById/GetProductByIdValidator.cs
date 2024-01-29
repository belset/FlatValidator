using System.Validation;


namespace Application.Products.Queries.GetProductById;

internal class GetProductByIdValidator : FlatValidator<GetProductByIdQuery>
{
    public GetProductByIdValidator(IProductRepository productRepository)
    {
        ErrorIf(x => x.Id.IsEmpty(), "Invalid Product ID.", x => x.Id);
        ValidIf(async x => await productRepository.ProductExists(x.Id), "Product not found.", x => x.Id);
    }
}
