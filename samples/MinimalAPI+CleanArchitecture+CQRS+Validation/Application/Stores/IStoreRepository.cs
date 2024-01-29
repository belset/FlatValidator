
namespace Application.Stores;

public interface IStoreRepository
{
    ValueTask<List<Entities.Store>> GetStores(CancellationToken cancellationToken = default);

#nullable enable
    ValueTask<Entities.Store?> GetStoreById(Guid id, CancellationToken cancellationToken = default);
    ValueTask<Entities.Store?> GetStoreByName(string storeName, CancellationToken cancellationToken = default);
#nullable disable

    ValueTask<bool> StoreExists(Guid id, CancellationToken cancellationToken = default);
    ValueTask<bool> StoreExists(string storeName, CancellationToken cancellationToken);
}
