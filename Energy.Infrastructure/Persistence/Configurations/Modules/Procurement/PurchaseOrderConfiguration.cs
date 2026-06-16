using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Procurement;

/// <summary>PurchaseOrder EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class PurchaseOrderConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Procurement.PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Procurement.PurchaseOrder> builder)
    {
        builder.ToTable("PurchaseOrders");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.BusinessPartners.BusinessPartner>().WithMany().HasForeignKey(e => e.SupplierId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Projects.Project>().WithMany().HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Restrict);
    }
}
