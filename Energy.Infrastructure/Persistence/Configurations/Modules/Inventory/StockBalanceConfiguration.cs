using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>StockBalance EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class StockBalanceConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Inventory.StockBalance>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Inventory.StockBalance> builder)
    {
        builder.ToTable("StockBalances");
        builder.HasKey(e => e.Id);
    }
}
