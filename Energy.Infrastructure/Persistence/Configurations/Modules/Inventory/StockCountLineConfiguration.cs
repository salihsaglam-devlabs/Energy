using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>StockCountLine EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class StockCountLineConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Inventory.StockCountLine>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Inventory.StockCountLine> builder)
    {
        builder.ToTable("StockCountLines");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.StockCount>().WithMany().HasForeignKey(e => e.StockCountId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.Material>().WithMany().HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}
