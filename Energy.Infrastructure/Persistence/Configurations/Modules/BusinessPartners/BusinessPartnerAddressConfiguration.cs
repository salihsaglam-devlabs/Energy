using Energy.Domain.Modules.BusinessPartners;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Organization;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.BusinessPartners;

/// <summary>BusinessPartnerAddress EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class BusinessPartnerAddressConfiguration : IEntityTypeConfiguration<BusinessPartnerAddress>
{
    public void Configure(EntityTypeBuilder<BusinessPartnerAddress> e)
    {
        e.ToTable("BusinessPartnerAddresses");
        e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.BusinessPartnerId).OnDelete(DeleteBehavior.Cascade);
    }
}
