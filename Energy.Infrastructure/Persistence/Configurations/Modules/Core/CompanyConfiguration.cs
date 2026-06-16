using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Core;

/// <summary>Company EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class CompanyConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Core.Company>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Core.Company> builder)
    {
        builder.ToTable("Companies");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Core.Currency>().WithMany().HasForeignKey(e => e.BaseCurrencyId).OnDelete(DeleteBehavior.Restrict);
    }
}
