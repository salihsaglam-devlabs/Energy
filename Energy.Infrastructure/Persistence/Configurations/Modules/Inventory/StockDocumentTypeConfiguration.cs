using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>StockDocumentType EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class StockDocumentTypeConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Inventory.StockDocumentType>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Inventory.StockDocumentType> builder)
    {
        builder.ToTable("StockDocumentTypes");
        builder.HasKey(e => e.Id);
    }
}
