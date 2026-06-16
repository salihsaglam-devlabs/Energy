using Energy.Domain.Catalog;
using Energy.Domain.Core;
using Energy.Domain.Inventory;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Inventory;

/// <summary>StockLot EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class StockLotConfiguration : IEntityTypeConfiguration<StockLot>
{
    public void Configure(EntityTypeBuilder<StockLot> e)
    {
        e.ToTable("StockLots");
        e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<StockDocumentLine>().WithMany().HasForeignKey(x => x.SourceStockDocumentLineId).OnDelete(DeleteBehavior.Restrict);
    }
}
