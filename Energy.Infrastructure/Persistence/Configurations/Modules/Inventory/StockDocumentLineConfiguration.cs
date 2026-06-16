using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>StockDocumentLine EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class StockDocumentLineConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Inventory.StockDocumentLine>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Inventory.StockDocumentLine> builder)
    {
        builder.ToTable("StockDocumentLines");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.StockDocument>().WithMany().HasForeignKey(e => e.StockDocumentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.Material>().WithMany().HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Core.UnitOfMeasure>().WithMany().HasForeignKey(e => e.UnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Core.Currency>().WithMany().HasForeignKey(e => e.CurrencyId).OnDelete(DeleteBehavior.Restrict);
    }
}
