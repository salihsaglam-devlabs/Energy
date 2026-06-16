using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.FieldOperations;

/// <summary>MeasurementSheet EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class MeasurementSheetConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.FieldOperations.MeasurementSheet>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.FieldOperations.MeasurementSheet> builder)
    {
        builder.ToTable("MeasurementSheets");
        builder.HasKey(e => e.Id);
    }
}
