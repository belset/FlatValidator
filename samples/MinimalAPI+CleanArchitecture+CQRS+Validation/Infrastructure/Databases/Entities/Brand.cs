namespace Infrastructure.Databases.Entities;

internal record Brand
{
    public Guid Id { get; init; }

    public DateTime DateCreated { get; init; }
    public DateTime DateModified { get; set; }

    public string BrandName { get; init; }

    public List<Product> Products { get; init; }
}
