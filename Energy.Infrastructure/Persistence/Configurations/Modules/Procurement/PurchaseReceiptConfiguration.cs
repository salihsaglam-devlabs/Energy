using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Procurement;

/// <summary>PurchaseReceipt EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class PurchaseReceiptConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Procurement.PurchaseReceipt>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Procurement.PurchaseReceipt> builder)
    {
        builder.ToTable("PurchaseReceipts");
        builder.HasKey(e => e.Id);
    }
}
