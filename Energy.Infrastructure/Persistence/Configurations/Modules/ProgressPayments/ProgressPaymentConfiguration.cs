using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.ProgressPayments;

/// <summary>ProgressPayment EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ProgressPaymentConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.ProgressPayments.ProgressPayment>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.ProgressPayments.ProgressPayment> builder)
    {
        builder.ToTable("ProgressPayments");
        builder.HasKey(e => e.Id);
    }
}
