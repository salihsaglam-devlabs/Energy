using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Core;

/// <summary>UnitOfMeasure EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class UnitOfMeasureConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Core.UnitOfMeasure>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Core.UnitOfMeasure> builder)
    {
        builder.ToTable("UnitsOfMeasure");
        builder.HasKey(e => e.Id);
    }
}
