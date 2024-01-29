using Infrastructure.Databases.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Databases.Configuration;

internal class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.DateCreated).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.DateModified).ValueGeneratedOnAdd().IsRequired();

        builder.HasMany(x => x.Products).WithOne(x => x.Brand).HasForeignKey(x => x.BrandId);

        builder.HasData([new Brand
        {
            Id = Guid.NewGuid(),
            BrandName = "123",
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow
        }]);
    }
}
