using System.Validation;

using Application.Brands;


namespace Application.Products.Commands.UpdateProduct;

internal class UpdateProductValidator : FlatValidator<UpdateProductCommand>
{
    public UpdateProductValidator(IProductRepository productRepository, IBrandRepository brandRepository)
    {
        ErrorIf(m => m.Id.IsEmpty(), "Invalid Product ID.", m => m.Id);
        ErrorIf(m => m.ProductName.IsEmpty(), "Product name cannot be empty.", m => m.ProductName);

        When(async m => await productRepository.ProductExists(m.Id), @then: m =>
        {
            ErrorIf(m => m.BrandName.IsEmpty(), "Brand name cannot be empty.", m => m.BrandName);
            ValidIf(m => brandRepository.BrandExists(m.BrandName),
                    m => $"Brand name '{m.BrandName}' not found.", 
                    m => m.BrandName);
        },
        @else: m =>
        {
            Error($"Product '{m.ProductName}' not found.", m => m.ProductName);
        });

        ValidIf(m => 1 <= m.Rate && m.Rate <= 5, "Invalid amount of stars.", m => m.Rate);
    }
}
