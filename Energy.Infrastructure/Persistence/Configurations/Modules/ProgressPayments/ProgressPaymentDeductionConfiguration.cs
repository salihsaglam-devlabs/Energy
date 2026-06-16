using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.ProgressPayments;

/// <summary>ProgressPaymentDeduction EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ProgressPaymentDeductionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.ProgressPayments.ProgressPaymentDeduction>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.ProgressPayments.ProgressPaymentDeduction> builder)
    {
        builder.ToTable("ProgressPaymentDeductions");
        builder.HasKey(e => e.Id);
    }
}
