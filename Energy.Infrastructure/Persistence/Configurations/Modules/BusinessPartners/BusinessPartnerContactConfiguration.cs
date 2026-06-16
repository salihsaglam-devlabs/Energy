using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.BusinessPartners;

/// <summary>BusinessPartnerContact EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class BusinessPartnerContactConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.BusinessPartners.BusinessPartnerContact>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.BusinessPartners.BusinessPartnerContact> builder)
    {
        builder.ToTable("BusinessPartnerContacts");
        builder.HasKey(e => e.Id);
    }
}
