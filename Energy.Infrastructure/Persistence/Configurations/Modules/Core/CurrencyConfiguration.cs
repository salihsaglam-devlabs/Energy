using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Core;

/// <summary>Currency EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class CurrencyConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Core.Currency>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Core.Currency> builder)
    {
        builder.ToTable("Currencies");
        builder.HasKey(e => e.Id);
    }
}
