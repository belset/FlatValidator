using System.Reflection;

using Application.Common.Services;

using Bogus;

using Infrastructure.Databases.Entities;
using Infrastructure.Databases.Extensions;

using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Databases;

internal class DataContext(DbContextOptions<DataContext> options) : DbContext(options), IDataInitialization
{
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Product> Products { get; set; }

    public void Initialize()
    {
        this.Seeds();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        _ = builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
