namespace Application.Products.Entities;

public record Product
{
    public Guid Id { get; init; }

    public DateTime DateCreated { get; init; }
    public DateTime DateModified { get; init; }

    public Guid BrandId { get; set; }

    public string ProductName { get; set; }

    public int Rate { get; set; }
}
