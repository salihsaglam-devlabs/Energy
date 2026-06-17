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

/// <summary>SupplierQuoteLine EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class SupplierQuoteLineConfiguration : IEntityTypeConfiguration<SupplierQuoteLine>
{
    public void Configure(EntityTypeBuilder<SupplierQuoteLine> e)
    {
        e.ToTable("SupplierQuoteLines");
        e.HasOne<SupplierQuote>().WithMany().HasForeignKey(x => x.SupplierQuoteId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<RequestLine>().WithMany().HasForeignKey(x => x.RequestLineId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}
