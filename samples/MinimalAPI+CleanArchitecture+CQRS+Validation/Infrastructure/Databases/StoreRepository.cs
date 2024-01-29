using Application.Stores;
using Application.Stores.Entities;

using AutoMapper;

using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Databases;

internal class StoreRepository : IStoreRepository
{
    private readonly DataContext context;
    private readonly TimeProvider timeProvider;
    private readonly IMapper mapper;

    public StoreRepository(DataContext context, TimeProvider timeProvider, IMapper mapper)
    {
        this.context = context;
        this.timeProvider = timeProvider;
        this.mapper = mapper;
    }

    public virtual async ValueTask<List<Store>> GetStores(CancellationToken cancellationToken)
    {
        var Stores = await context.Stores.Include(x => x.Products).ThenInclude(x => x.Brand).AsNoTracking().ToListAsync(cancellationToken);

        return mapper.Map<List<Store>>(Stores);
    }

#nullable enable
    public virtual async ValueTask<Store?> GetStoreById(Guid id, CancellationToken cancellationToken)
    {
        var Store = await context.Stores.Where(x => x.Id == id).Include(x => x.Products).ThenInclude(x => x.Brand).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        return mapper.Map<Store>(Store);
    }

    public virtual async ValueTask<Store?> GetStoreByName(string storeName, CancellationToken cancellationToken)
    {
        var Store = await context.Stores.Where(x => x.StoreName == storeName).Include(x => x.Products).ThenInclude(x => x.Brand).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        return mapper.Map<Store>(Store);
    }
#nullable disable

    public virtual async ValueTask<bool> StoreExists(Guid id, CancellationToken cancellationToken)
    {
        return await context.Stores.AnyAsync(x => x.Id == id, cancellationToken);
    }
    public virtual async ValueTask<bool> StoreExists(string storeName, CancellationToken cancellationToken)
    {
        return await context.Stores.AnyAsync(x => x.StoreName == storeName, cancellationToken);
    }
}
