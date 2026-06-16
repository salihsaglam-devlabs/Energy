using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Procurement;

/// <summary>SupplierQuote EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class SupplierQuoteConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Procurement.SupplierQuote>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Procurement.SupplierQuote> builder)
    {
        builder.ToTable("SupplierQuotes");
        builder.HasKey(e => e.Id);
    }
}
