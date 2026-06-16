using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Finance;

/// <summary>PaymentAllocation EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class PaymentAllocationConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Finance.PaymentAllocation>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Finance.PaymentAllocation> builder)
    {
        builder.ToTable("PaymentAllocations");
        builder.HasKey(e => e.Id);
    }
}
