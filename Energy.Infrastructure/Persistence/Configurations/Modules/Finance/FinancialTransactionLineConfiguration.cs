using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Finance;

/// <summary>FinancialTransactionLine EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class FinancialTransactionLineConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Finance.FinancialTransactionLine>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Finance.FinancialTransactionLine> builder)
    {
        builder.ToTable("FinancialTransactionLines");
        builder.HasKey(e => e.Id);
    }
}
