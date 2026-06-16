using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Procurement;

/// <summary>PurchaseReceiptLine EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class PurchaseReceiptLineConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Procurement.PurchaseReceiptLine>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Procurement.PurchaseReceiptLine> builder)
    {
        builder.ToTable("PurchaseReceiptLines");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Procurement.PurchaseReceipt>().WithMany().HasForeignKey(e => e.PurchaseReceiptId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.Procurement.PurchaseOrderLine>().WithMany().HasForeignKey(e => e.PurchaseOrderLineId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.Material>().WithMany().HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}
