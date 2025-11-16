using System.Runtime.CompilerServices;

using Application.Brands;
using Application.Common.Services;
using Application.Products;
using Application.Stores;

using Infrastructure.Databases;
using Infrastructure.Databases.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure._Startup;

public static class StartupExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());
        services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase($"data-{Guid.NewGuid()}"), ServiceLifetime.Singleton);
        services.AddSingleton<IDataInitialization>(x => x.GetRequiredService<DataContext>());

        _ = services.AddSingleton<ProductRepository>();
        _ = services.AddSingleton<BrandRepository>();
        _ = services.AddSingleton<StoreRepository>();

        services.AddSingleton<IProductRepository>(x => x.GetRequiredService<ProductRepository>());
        services.AddSingleton<IBrandRepository>(x => x.GetRequiredService<BrandRepository>());
        services.AddSingleton<IStoreRepository>(x => x.GetRequiredService<StoreRepository>());

        services.AddSingleton(TimeProvider.System);

        return services;
    }
}
