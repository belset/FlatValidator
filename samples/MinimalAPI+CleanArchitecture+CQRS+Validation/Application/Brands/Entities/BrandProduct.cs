namespace Application.Brands.Entities;

public record BrandProduct
{
    public Guid Id { get; init; }

    public DateTime DateCreated { get; init; }
    public DateTime DateModified { get; init; }

    public string ProductName { get; init; }
}
