using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Core;

/// <summary>UnitConversion EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class UnitConversionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Core.UnitConversion>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Core.UnitConversion> builder)
    {
        builder.ToTable("UnitConversions");
        builder.HasKey(e => e.Id);
    }
}
