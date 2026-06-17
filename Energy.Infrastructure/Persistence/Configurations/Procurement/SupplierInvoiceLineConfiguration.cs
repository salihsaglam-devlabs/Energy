using Energy.Domain.BusinessPartners;
using Energy.Domain.Catalog;
using Energy.Domain.Core;
using Energy.Domain.IAM;
using Energy.Domain.Inventory;
using Energy.Domain.Procurement;
using Energy.Domain.Projects;
using Energy.Domain.Requests;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Procurement;

/// <summary>SupplierInvoiceLine EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class SupplierInvoiceLineConfiguration : IEntityTypeConfiguration<SupplierInvoiceLine>
{
    public void Configure(EntityTypeBuilder<SupplierInvoiceLine> e)
    {
        e.ToTable("SupplierInvoiceLines");
        e.HasOne<SupplierInvoice>().WithMany().HasForeignKey(x => x.SupplierInvoiceId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}
