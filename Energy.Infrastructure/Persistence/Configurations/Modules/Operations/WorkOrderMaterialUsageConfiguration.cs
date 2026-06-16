using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Operations;

/// <summary>WorkOrderMaterialUsage EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class WorkOrderMaterialUsageConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Operations.WorkOrderMaterialUsage>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Operations.WorkOrderMaterialUsage> builder)
    {
        builder.ToTable("WorkOrderMaterialUsages");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Operations.WorkOrder>().WithMany().HasForeignKey(e => e.WorkOrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.StockDocumentLine>().WithMany().HasForeignKey(e => e.StockDocumentLineId).OnDelete(DeleteBehavior.Restrict);
    }
}
