using Energy.Domain.Modules.Catalog;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.Inventory;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>StockBalance EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class StockBalanceConfiguration : IEntityTypeConfiguration<StockBalance>
{
    public void Configure(EntityTypeBuilder<StockBalance> e)
    {
        e.ToTable("StockBalances");
        e.HasIndex(x => new { x.WarehouseId, x.MaterialId }).IsUnique();
        e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}
