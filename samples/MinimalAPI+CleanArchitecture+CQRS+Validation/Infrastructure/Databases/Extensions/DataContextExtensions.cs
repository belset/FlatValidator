namespace Infrastructure.Databases.Extensions;

using System;

using Bogus;

using Infrastructure.Databases;
using Infrastructure.Databases.Entities;

internal static class DataContextExtensions
{
    public static DataContext Seeds(this DataContext context)
    {
        var stores = new Faker<Store>()
            .RuleFor(a => a.Id, _ => Guid.NewGuid())
            .RuleFor(a => a.DateCreated, f => f.Date.Past())
            .RuleFor(a => a.DateModified, f => f.Date.Past())
            .RuleFor(a => a.StoreName, f => f.Company.CompanyName())
            .Generate(5);

        context.AddRange(stores);

        var brands = new Faker<Brand>()
            .RuleFor(m => m.Id, _ => Guid.NewGuid())
            .RuleFor(a => a.DateCreated, f => f.Date.Past())
            .RuleFor(a => a.DateModified, f => f.Date.Past())
            .RuleFor(a => a.BrandName, f => f.Company.CompanyName())
            .Generate(3);

        context.AddRange(brands);

        var products = new Faker<Product>()
            .RuleFor(r => r.Id, _ => Guid.NewGuid())
            .RuleFor(r => r.DateCreated, f => f.Date.Past())
            .RuleFor(r => r.DateModified, f => f.Date.Past())
            .RuleFor(r => r.BrandId, f => f.PickRandom(brands).Id)
            .RuleFor(r => r.ProductName, f => f.Commerce.Product())
            .RuleFor(r => r.Rate, f => f.Random.Number(1, 5))
            .Generate(20);

        context.AddRange(products);

        _ = context.SaveChanges();

        return context;
    }
}
