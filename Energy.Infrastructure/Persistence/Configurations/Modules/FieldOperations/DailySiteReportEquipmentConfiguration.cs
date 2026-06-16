using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.FieldOperations;

/// <summary>DailySiteReportEquipment EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class DailySiteReportEquipmentConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.FieldOperations.DailySiteReportEquipment>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.FieldOperations.DailySiteReportEquipment> builder)
    {
        builder.ToTable("DailySiteReportEquipments");
        builder.HasKey(e => e.Id);
    }
}
