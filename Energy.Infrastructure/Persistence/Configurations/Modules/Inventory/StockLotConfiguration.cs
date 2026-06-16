using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>StockLot EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class StockLotConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Inventory.StockLot>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Inventory.StockLot> builder)
    {
        builder.ToTable("StockLots");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.Warehouse>().WithMany().HasForeignKey(e => e.WarehouseId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.Material>().WithMany().HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.StockDocumentLine>().WithMany().HasForeignKey(e => e.SourceStockDocumentLineId).OnDelete(DeleteBehavior.Restrict);
    }
}
