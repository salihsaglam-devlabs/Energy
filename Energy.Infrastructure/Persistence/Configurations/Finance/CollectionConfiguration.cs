using Energy.Domain.Budget;
using Energy.Domain.BusinessPartners;
using Energy.Domain.Contracts;
using Energy.Domain.Core;
using Energy.Domain.FieldOperations;
using Energy.Domain.Finance;
using Energy.Domain.ProgressPayments;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Finance;

/// <summary>Collection EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class CollectionConfiguration : IEntityTypeConfiguration<Collection>
{
    public void Configure(EntityTypeBuilder<Collection> e)
    {
        e.ToTable("Collections");
        e.HasIndex(x => x.CollectionNo).IsUnique();
        e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.PartnerId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<FinancialAccount>().WithMany().HasForeignKey(x => x.FinancialAccountId).OnDelete(DeleteBehavior.Restrict);
    }
}
