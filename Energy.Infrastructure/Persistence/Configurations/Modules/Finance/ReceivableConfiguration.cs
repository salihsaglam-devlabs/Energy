using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Finance;

/// <summary>Receivable EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ReceivableConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Finance.Receivable>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Finance.Receivable> builder)
    {
        builder.ToTable("Receivables");
        builder.HasKey(e => e.Id);
    }
}
