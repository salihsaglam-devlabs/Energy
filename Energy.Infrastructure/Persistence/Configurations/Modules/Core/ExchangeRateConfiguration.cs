using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.IAM;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Core;

/// <summary>ExchangeRate EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRate>
{
    public void Configure(EntityTypeBuilder<ExchangeRate> e)
    {
        e.ToTable("ExchangeRates");
        e.HasIndex(x => new { x.CurrencyId, x.RateDate }).IsUnique();
        e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
    }
}
