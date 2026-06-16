using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.BusinessPartners;

/// <summary>BusinessPartnerBankAccount EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class BusinessPartnerBankAccountConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.BusinessPartners.BusinessPartnerBankAccount>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.BusinessPartners.BusinessPartnerBankAccount> builder)
    {
        builder.ToTable("BusinessPartnerBankAccounts");
        builder.HasKey(e => e.Id);
    }
}
