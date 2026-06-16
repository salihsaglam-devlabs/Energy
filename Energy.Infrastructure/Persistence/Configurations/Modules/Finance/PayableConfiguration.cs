using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Finance;

/// <summary>Payable EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class PayableConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Finance.Payable>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Finance.Payable> builder)
    {
        builder.ToTable("Payables");
        builder.HasKey(e => e.Id);
    }
}
