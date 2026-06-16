using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.FieldOperations;

/// <summary>MeasurementSheetLine EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class MeasurementSheetLineConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.FieldOperations.MeasurementSheetLine>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.FieldOperations.MeasurementSheetLine> builder)
    {
        builder.ToTable("MeasurementSheetLines");
        builder.HasKey(e => e.Id);
    }
}
