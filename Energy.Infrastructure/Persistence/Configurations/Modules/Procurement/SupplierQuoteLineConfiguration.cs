using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Procurement;

/// <summary>SupplierQuoteLine EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class SupplierQuoteLineConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Procurement.SupplierQuoteLine>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Procurement.SupplierQuoteLine> builder)
    {
        builder.ToTable("SupplierQuoteLines");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Procurement.SupplierQuote>().WithMany().HasForeignKey(e => e.SupplierQuoteId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.Requests.RequestLine>().WithMany().HasForeignKey(e => e.RequestLineId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.Material>().WithMany().HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}
