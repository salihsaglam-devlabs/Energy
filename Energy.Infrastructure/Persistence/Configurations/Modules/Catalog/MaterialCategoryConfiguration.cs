using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Catalog;

/// <summary>MaterialCategory EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class MaterialCategoryConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Catalog.MaterialCategory>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Catalog.MaterialCategory> builder)
    {
        builder.ToTable("MaterialCategories");
        builder.HasKey(e => e.Id);
    }
}
