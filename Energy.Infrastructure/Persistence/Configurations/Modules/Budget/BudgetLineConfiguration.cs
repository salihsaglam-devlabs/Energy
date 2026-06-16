using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Budget;

/// <summary>BudgetLine EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class BudgetLineConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Budget.BudgetLine>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Budget.BudgetLine> builder)
    {
        builder.ToTable("BudgetLines");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Budget.Budget>().WithMany().HasForeignKey(e => e.BudgetId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.Projects.Project>().WithMany().HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Restrict);
    }
}
