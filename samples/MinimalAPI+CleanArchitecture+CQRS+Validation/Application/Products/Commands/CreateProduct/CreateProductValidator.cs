using System.Validation;

using Application.Brands;


namespace Application.Products.Commands.CreateProduct;

internal class CreateProductValidator : FlatValidator<CreateProductCommand>
{
    public CreateProductValidator(IProductRepository productRepository, IBrandRepository brandRepository)
    {
        ErrorIf(m => m.ProductName.IsEmpty(), "Product name cannot be empty.", m => m.ProductName);
        ErrorIf(m => m.BrandName.IsEmpty(), "Brand name cannot be empty.", m => m.BrandName);

        When(m => brandRepository.BrandExists(m.BrandName), @then: m =>
        {
            ErrorIf(m => productRepository.ProductExists(m.ProductName, m.BrandName), 
                    m => $"Product '{m.ProductName}' for '{m.BrandName}' already exists.", 
                    m => m.ProductName, m => m.BrandName);
        },
        @else: m =>
        {
            Error($"Brand '{m.BrandName}' not found.", m => m.BrandName);
        });

        ErrorIf(m => m.Rate < 1, m => $"Too small rate value: {m.Rate}.", m => m.Rate);
        ErrorIf(m => m.Rate > 5, m => $"Too big rate value: {m.Rate}", m => m.Rate);
    }
}
