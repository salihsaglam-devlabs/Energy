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

/// <summary>SupplierQuote EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class SupplierQuoteConfiguration : IEntityTypeConfiguration<SupplierQuote>
{
    public void Configure(EntityTypeBuilder<SupplierQuote> e)
    {
        e.ToTable("SupplierQuotes");
        e.HasIndex(x => x.QuoteNo).IsUnique();
        e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
    }
}
