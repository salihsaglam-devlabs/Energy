using Energy.Domain.Modules.BusinessPartners;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Organization;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.BusinessPartners;

/// <summary>BusinessPartnerContact EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class BusinessPartnerContactConfiguration : IEntityTypeConfiguration<BusinessPartnerContact>
{
    public void Configure(EntityTypeBuilder<BusinessPartnerContact> e)
    {
        e.ToTable("BusinessPartnerContacts");
        e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.BusinessPartnerId).OnDelete(DeleteBehavior.Cascade);
    }
}
