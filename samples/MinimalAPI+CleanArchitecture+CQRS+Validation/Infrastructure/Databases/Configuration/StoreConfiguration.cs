using Infrastructure.Databases.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Databases.Configuration;

internal class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.DateCreated).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.DateModified).ValueGeneratedOnAdd().IsRequired();
    }
}
