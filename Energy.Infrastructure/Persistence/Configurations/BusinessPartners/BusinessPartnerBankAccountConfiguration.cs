using Energy.Domain.BusinessPartners;
using Energy.Domain.Core;
using Energy.Domain.IAM;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.BusinessPartners;

/// <summary>BusinessPartnerBankAccount EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class BusinessPartnerBankAccountConfiguration : IEntityTypeConfiguration<BusinessPartnerBankAccount>
{
    public void Configure(EntityTypeBuilder<BusinessPartnerBankAccount> e)
    {
        e.ToTable("BusinessPartnerBankAccounts");
        e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.BusinessPartnerId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
    }
}
