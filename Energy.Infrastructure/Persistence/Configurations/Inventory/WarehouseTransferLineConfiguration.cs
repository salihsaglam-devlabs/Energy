using Energy.Domain.Catalog;
using Energy.Domain.Core;
using Energy.Domain.Inventory;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Inventory;

/// <summary>WarehouseTransferLine EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class WarehouseTransferLineConfiguration : IEntityTypeConfiguration<WarehouseTransferLine>
{
    public void Configure(EntityTypeBuilder<WarehouseTransferLine> e)
    {
        e.ToTable("WarehouseTransferLines");
        e.HasOne<WarehouseTransfer>().WithMany().HasForeignKey(x => x.WarehouseTransferId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}
