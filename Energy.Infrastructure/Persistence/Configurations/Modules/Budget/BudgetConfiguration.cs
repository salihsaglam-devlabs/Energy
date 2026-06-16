using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Budget;

/// <summary>Budget EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class BudgetConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Budget.Budget>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Budget.Budget> builder)
    {
        builder.ToTable("Budgets");
        builder.HasKey(e => e.Id);
    }
}
