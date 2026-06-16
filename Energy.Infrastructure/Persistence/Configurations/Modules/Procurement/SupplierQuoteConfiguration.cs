using Energy.Domain.Modules.BusinessPartners;
using Energy.Domain.Modules.Catalog;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Inventory;
using Energy.Domain.Modules.Procurement;
using Energy.Domain.Modules.Projects;
using Energy.Domain.Modules.Requests;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Procurement;

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
