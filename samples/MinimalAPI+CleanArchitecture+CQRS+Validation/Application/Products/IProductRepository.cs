using Application.Products.Entities;


namespace Application.Products;

public interface IProductRepository
{
    ValueTask<List<Product>> GetProducts(CancellationToken cancellationToken);

#nullable enable
    ValueTask<Product?> GetProductById(Guid id, CancellationToken cancellationToken = default);
    ValueTask<Product?> GetProductByName(string productName, string brandName, CancellationToken cancellationToken = default);
#nullable disable

    ValueTask<bool> ProductExists(Guid id, CancellationToken cancellationToken = default);
    ValueTask<bool> ProductExists(string productName, string brandName, CancellationToken cancellationToken = default);

    ValueTask<Product> CreateProduct(string productName, int stars, Guid brandId, CancellationToken cancellationToken = default);
    ValueTask<bool> DeleteProduct(Guid id, CancellationToken cancellationToken = default);
    ValueTask<bool> UpdateProduct(Guid id, string productName, int stars, CancellationToken cancellationToken = default);
}
