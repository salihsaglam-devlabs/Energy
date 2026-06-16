using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.BusinessPartners;

/// <summary>BusinessPartner EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class BusinessPartnerConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.BusinessPartners.BusinessPartner>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.BusinessPartners.BusinessPartner> builder)
    {
        builder.ToTable("BusinessPartners");
        builder.HasKey(e => e.Id);
    }
}
