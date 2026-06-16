using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Procurement;

/// <summary>SupplierInvoiceLine EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class SupplierInvoiceLineConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Procurement.SupplierInvoiceLine>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Procurement.SupplierInvoiceLine> builder)
    {
        builder.ToTable("SupplierInvoiceLines");
        builder.HasKey(e => e.Id);
    }
}
