using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Catalog;

/// <summary>MaterialUnitConversion EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class MaterialUnitConversionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Catalog.MaterialUnitConversion>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Catalog.MaterialUnitConversion> builder)
    {
        builder.ToTable("MaterialUnitConversions");
        builder.HasKey(e => e.Id);
    }
}
