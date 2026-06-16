using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Procurement;

/// <summary>PurchaseOrderLine EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class PurchaseOrderLineConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Procurement.PurchaseOrderLine>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Procurement.PurchaseOrderLine> builder)
    {
        builder.ToTable("PurchaseOrderLines");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Procurement.PurchaseOrder>().WithMany().HasForeignKey(e => e.PurchaseOrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Requests.RequestLine>().WithMany().HasForeignKey(e => e.RequestLineId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.Material>().WithMany().HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Core.Currency>().WithMany().HasForeignKey(e => e.CurrencyId).OnDelete(DeleteBehavior.Restrict);
    }
}
