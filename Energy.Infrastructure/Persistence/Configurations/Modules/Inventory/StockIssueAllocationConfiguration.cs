using Energy.Domain.Modules.Catalog;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.Inventory;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>StockIssueAllocation EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class StockIssueAllocationConfiguration : IEntityTypeConfiguration<StockIssueAllocation>
{
    public void Configure(EntityTypeBuilder<StockIssueAllocation> e)
    {
        e.ToTable("StockIssueAllocations");
        e.HasOne<StockDocumentLine>().WithMany().HasForeignKey(x => x.StockDocumentLineId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<StockLot>().WithMany().HasForeignKey(x => x.StockLotId).OnDelete(DeleteBehavior.Restrict);
    }
}
