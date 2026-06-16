using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.BusinessPartners;

/// <summary>BusinessPartnerAddress EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class BusinessPartnerAddressConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.BusinessPartners.BusinessPartnerAddress>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.BusinessPartners.BusinessPartnerAddress> builder)
    {
        builder.ToTable("BusinessPartnerAddresses");
        builder.HasKey(e => e.Id);
    }
}
