using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>WarehouseTransferLine EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class WarehouseTransferLineConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Inventory.WarehouseTransferLine>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Inventory.WarehouseTransferLine> builder)
    {
        builder.ToTable("WarehouseTransferLines");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.WarehouseTransfer>().WithMany().HasForeignKey(e => e.WarehouseTransferId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.Material>().WithMany().HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}
