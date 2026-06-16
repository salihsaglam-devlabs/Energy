using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Procurement;

/// <summary>SupplierInvoice EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class SupplierInvoiceConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Procurement.SupplierInvoice>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Procurement.SupplierInvoice> builder)
    {
        builder.ToTable("SupplierInvoices");
        builder.HasKey(e => e.Id);
    }
}
