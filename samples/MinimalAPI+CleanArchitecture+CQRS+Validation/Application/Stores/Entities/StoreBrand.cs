namespace Application.Stores.Entities;

public record StoreBrand
{
    public Guid Id { get; init; }

    public DateTime DateCreated { get; init; }
    public DateTime DateModified { get; init; }

    public string StoreName { get; set; }

    public string PostalAddress { get; set; }
}
