using Application.Products;
using Application.Products.Entities;

using AutoMapper;

using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Databases;

internal class ProductRepository : IProductRepository
{
    private readonly DataContext context;
    private readonly TimeProvider timeProvider;
    private readonly IMapper mapper;

    public ProductRepository(DataContext context, TimeProvider timeProvider, IMapper mapper)
    {
        this.context = context;
        this.timeProvider = timeProvider;
        this.mapper = mapper;
    }

    public virtual async ValueTask<List<Product>> GetProducts(CancellationToken cancellationToken)
    {
        var products = await context.Products
            .Include(x => x.Brand)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return mapper.Map<List<Product>>(products);
    }

#nullable enable
    public virtual async ValueTask<Product?> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var product = await context.Products.Include(x => x.Brand)
            .Where(x => x.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return mapper.Map<Product>(product);
    }
    public virtual async ValueTask<Product?> GetProductByName(string productName, string brandName, CancellationToken cancellationToken)
    {
        var product = await context.Products.Include(x => x.Brand)
            .Where(x => x.ProductName == productName && x.Brand.BrandName == brandName)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return mapper.Map<Product>(product);
    }
#nullable disable

    public virtual async ValueTask<bool> ProductExists(Guid id, CancellationToken cancellationToken)
    {
        return await context.Products.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public virtual async ValueTask<bool> ProductExists(string productName, string brandName, CancellationToken cancellationToken)
    {
        return await context.Products.Include(x => x.Brand)
            .AnyAsync(x => x.ProductName == productName && x.Brand.BrandName == brandName, cancellationToken);
    }

    public virtual async ValueTask<Product> CreateProduct(string productName, int stars, Guid brandId, CancellationToken cancellationToken)
    {
        var product = new Infrastructure.Databases.Entities.Product
        {
            DateCreated = timeProvider.GetUtcNow().UtcDateTime,
            DateModified = timeProvider.GetUtcNow().UtcDateTime,

            ProductName = productName,
            Rate = stars,

            BrandId = brandId,
        };
        var id = context.Add(product).Entity.Id;

        await context.SaveChangesAsync(cancellationToken);

        var result = await context.Products.Include(x => x.Brand)
            .Where(x => x.Id == id)
            .AsNoTracking()
            .FirstAsync(cancellationToken);

        return mapper.Map<Product>(result);
    }

    public virtual async ValueTask<bool> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            context.Remove(context.Products.Single(x => x.Id == id));
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (Exception)
        {
        }
        return false;
    }

    public virtual async ValueTask<bool> UpdateProduct(Guid id, string productName, int stars, CancellationToken cancellationToken)
    {
        try
        {
            var product = context.Products.FirstOrDefault(r => r.Id == id);
            if (product is not null)
            {
                product.ProductName = productName;
                product.Rate = stars;
                product.DateModified = timeProvider.GetUtcNow().UtcDateTime;

                context.Update(product);
                return await context.SaveChangesAsync(cancellationToken) == 1;
            }
        }
        catch (Exception)
        {
        }
        return false;
    }
}
