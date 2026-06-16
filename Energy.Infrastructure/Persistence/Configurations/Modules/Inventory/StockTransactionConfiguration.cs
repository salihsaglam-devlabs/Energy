using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>StockTransaction EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class StockTransactionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Inventory.StockTransaction>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Inventory.StockTransaction> builder)
    {
        builder.ToTable("StockTransactions");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.StockDocument>().WithMany().HasForeignKey(e => e.StockDocumentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.StockDocumentLine>().WithMany().HasForeignKey(e => e.StockDocumentLineId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.StockLot>().WithMany().HasForeignKey(e => e.StockLotId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.Warehouse>().WithMany().HasForeignKey(e => e.WarehouseId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.Material>().WithMany().HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}
