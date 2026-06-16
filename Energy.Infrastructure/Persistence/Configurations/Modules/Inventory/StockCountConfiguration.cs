using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>StockCount EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class StockCountConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Inventory.StockCount>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Inventory.StockCount> builder)
    {
        builder.ToTable("StockCounts");
        builder.HasKey(e => e.Id);
    }
}
