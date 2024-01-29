using Application.Brands;
using Application.Brands.Entities;

using AutoMapper;

using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Databases;

internal class BrandRepository : IBrandRepository
{
    private readonly DataContext context;
    private readonly TimeProvider timeProvider;
    private readonly IMapper mapper;

    public BrandRepository(DataContext context, TimeProvider timeProvider, IMapper mapper)
    {
        this.context = context;
        this.timeProvider = timeProvider;
        this.mapper = mapper;
    }

    public virtual async ValueTask<List<Brand>> GetBrands(CancellationToken cancellationToken)
    {
        var brands = await context.Brands.Include(x => x.Products).AsNoTracking().ToListAsync(cancellationToken);

        return mapper.Map<List<Brand>>(brands);
    }

#nullable enable
    public virtual async ValueTask<Brand?> GetBrandById(Guid id, CancellationToken cancellationToken)
    {
        var brand = await context.Brands.Where(x => x.Id == id).Include(x => x.Products).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        return mapper.Map<Brand>(brand);
    }

    public virtual async ValueTask<Brand?> GetBrandByName(string brandName, CancellationToken cancellationToken)
    {
        var brand = await context.Brands.Where(x => x.BrandName == brandName).Include(x => x.Products).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        return mapper.Map<Brand>(brand);
    }
#nullable disable

    public virtual async ValueTask<bool> BrandExists(Guid id, CancellationToken cancellationToken)
    {
        return await context.Brands.AnyAsync(x => x.Id == id, cancellationToken);
    }
    public virtual async ValueTask<bool> BrandExists(string brandName, CancellationToken cancellationToken)
    {
        return await context.Brands.AnyAsync(x => x.BrandName == brandName, cancellationToken);
    }
}
