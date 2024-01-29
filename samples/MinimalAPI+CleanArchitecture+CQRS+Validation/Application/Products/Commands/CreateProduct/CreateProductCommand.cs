namespace Application.Products.Commands.CreateProduct;

public class CreateProductCommand
{
    public required string ProductName { get; set; }
    public required string BrandName { get; set; }

    public int Rate { get; set; } = 0;
}
