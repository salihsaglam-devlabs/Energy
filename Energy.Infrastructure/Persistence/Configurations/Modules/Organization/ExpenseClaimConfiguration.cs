using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Organization;

/// <summary>ExpenseClaim EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ExpenseClaimConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Organization.ExpenseClaim>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Organization.ExpenseClaim> builder)
    {
        builder.ToTable("ExpenseClaims");
        builder.HasKey(e => e.Id);
    }
}
