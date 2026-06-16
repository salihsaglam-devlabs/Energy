using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Organization;

/// <summary>ExpenseClaimLine EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ExpenseClaimLineConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Organization.ExpenseClaimLine>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Organization.ExpenseClaimLine> builder)
    {
        builder.ToTable("ExpenseClaimLines");
        builder.HasKey(e => e.Id);
    }
}
