using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Catalog;

/// <summary>Brand EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class BrandConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Catalog.Brand>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Catalog.Brand> builder)
    {
        builder.ToTable("Brands");
        builder.HasKey(e => e.Id);
    }
}
