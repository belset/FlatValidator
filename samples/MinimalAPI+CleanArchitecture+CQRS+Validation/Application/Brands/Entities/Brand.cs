namespace Application.Brands.Entities;

public record Brand
{
    public Guid Id { get; init; }

    public DateTime DateCreated { get; init; }
    public DateTime DateModified { get; init; }

    public string BrandName { get; init; }

    public ICollection<BrandProduct> Products { get; init; }
}
