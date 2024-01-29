namespace Infrastructure.Databases.Entities;

internal record Store
{
    public Guid Id { get; init; }

    public DateTime DateCreated { get; init; }
    public DateTime DateModified { get; set; }

    public string StoreName { get; init; }

    public ICollection<Product> Products { get; init; }
}
