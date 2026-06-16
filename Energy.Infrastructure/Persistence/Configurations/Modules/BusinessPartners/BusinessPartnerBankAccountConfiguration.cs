using Energy.Domain.Modules.BusinessPartners;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Organization;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.BusinessPartners;

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
