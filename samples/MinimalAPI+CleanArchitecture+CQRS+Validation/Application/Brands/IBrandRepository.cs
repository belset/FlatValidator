using Application.Brands.Entities;


namespace Application.Brands;

public interface IBrandRepository
{
    ValueTask<List<Brand>> GetBrands(CancellationToken cancellationToken = default);

#nullable enable
    ValueTask<Brand?> GetBrandById(Guid id, CancellationToken cancellationToken = default);
    ValueTask<Brand?> GetBrandByName(string brandName, CancellationToken cancellationToken = default);
#nullable disable

    ValueTask<bool> BrandExists(Guid id, CancellationToken cancellationToken = default);
    ValueTask<bool> BrandExists(string brandName, CancellationToken cancellationToken = default);
}
