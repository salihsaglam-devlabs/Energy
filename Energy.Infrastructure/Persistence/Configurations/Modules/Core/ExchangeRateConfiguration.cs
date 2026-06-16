using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Core;

/// <summary>ExchangeRate EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ExchangeRateConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Core.ExchangeRate>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Core.ExchangeRate> builder)
    {
        builder.ToTable("ExchangeRates");
        builder.HasKey(e => e.Id);
    }
}
