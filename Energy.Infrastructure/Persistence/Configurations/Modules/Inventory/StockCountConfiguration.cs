using Energy.Domain.Modules.Catalog;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.Inventory;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>StockCount EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class StockCountConfiguration : IEntityTypeConfiguration<StockCount>
{
    public void Configure(EntityTypeBuilder<StockCount> e)
    {
        e.ToTable("StockCounts");
        e.HasIndex(x => x.CountNo).IsUnique();
        e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
    }
}
