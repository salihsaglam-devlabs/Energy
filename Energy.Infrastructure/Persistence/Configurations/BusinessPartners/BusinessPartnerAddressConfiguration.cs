using Energy.Domain.BusinessPartners;
using Energy.Domain.Core;
using Energy.Domain.IAM;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.BusinessPartners;

/// <summary>BusinessPartnerAddress EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class BusinessPartnerAddressConfiguration : IEntityTypeConfiguration<BusinessPartnerAddress>
{
    public void Configure(EntityTypeBuilder<BusinessPartnerAddress> e)
    {
        e.ToTable("BusinessPartnerAddresses");
        e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.BusinessPartnerId).OnDelete(DeleteBehavior.Cascade);
    }
}
