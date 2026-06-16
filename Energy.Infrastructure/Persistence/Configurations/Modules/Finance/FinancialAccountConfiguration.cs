using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Finance;

/// <summary>FinancialAccount EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class FinancialAccountConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Finance.FinancialAccount>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Finance.FinancialAccount> builder)
    {
        builder.ToTable("FinancialAccounts");
        builder.HasKey(e => e.Id);
    }
}
