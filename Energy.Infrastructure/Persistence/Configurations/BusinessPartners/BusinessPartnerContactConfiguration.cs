using Energy.Domain.BusinessPartners;
using Energy.Domain.Core;
using Energy.Domain.IAM;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.BusinessPartners;

/// <summary>BusinessPartnerContact EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class BusinessPartnerContactConfiguration : IEntityTypeConfiguration<BusinessPartnerContact>
{
    public void Configure(EntityTypeBuilder<BusinessPartnerContact> e)
    {
        e.ToTable("BusinessPartnerContacts");
        e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.BusinessPartnerId).OnDelete(DeleteBehavior.Cascade);
    }
}
