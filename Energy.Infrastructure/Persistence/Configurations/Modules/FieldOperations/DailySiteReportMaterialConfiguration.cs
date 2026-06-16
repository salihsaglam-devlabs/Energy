using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.FieldOperations;

/// <summary>DailySiteReportMaterial EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class DailySiteReportMaterialConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.FieldOperations.DailySiteReportMaterial>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.FieldOperations.DailySiteReportMaterial> builder)
    {
        builder.ToTable("DailySiteReportMaterials");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.FieldOperations.DailySiteReport>().WithMany().HasForeignKey(e => e.DailySiteReportId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.Material>().WithMany().HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}
