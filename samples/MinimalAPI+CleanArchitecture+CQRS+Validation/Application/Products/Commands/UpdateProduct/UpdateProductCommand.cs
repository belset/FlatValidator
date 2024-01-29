namespace Application.Products.Commands.UpdateProduct;

public class UpdateProductCommand
{
    public required Guid Id { get; set; }
    public required string ProductName { get; set; }
    public required string BrandName { get; set; }
    
    public required int Rate { get; set; }
}
