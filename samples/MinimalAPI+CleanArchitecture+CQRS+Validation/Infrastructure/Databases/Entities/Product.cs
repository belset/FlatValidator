using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Databases.Entities;

internal record Product
{
    public Guid Id { get; init; }

    public DateTime DateCreated { get; init; }
    public DateTime DateModified { get; set; }

    public string ProductName { get; set; }
    public int Rate { get; set; }


    public Guid BrandId { get; set; }

    [InverseProperty(nameof(Store.Products))]
    public Brand Brand { get; init; }
}
