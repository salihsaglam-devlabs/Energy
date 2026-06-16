using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Finance;

/// <summary>Payment EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class PaymentConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Finance.Payment>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Finance.Payment> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(e => e.Id);
    }
}
