using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>StockIssueAllocation EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class StockIssueAllocationConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Inventory.StockIssueAllocation>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Inventory.StockIssueAllocation> builder)
    {
        builder.ToTable("StockIssueAllocations");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.StockDocumentLine>().WithMany().HasForeignKey(e => e.StockDocumentLineId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.StockLot>().WithMany().HasForeignKey(e => e.StockLotId).OnDelete(DeleteBehavior.Restrict);
    }
}
