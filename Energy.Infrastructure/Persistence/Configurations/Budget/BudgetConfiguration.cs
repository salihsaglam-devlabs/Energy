using BudgetEntity = Energy.Domain.Budget.Budget;
using Energy.Domain.Budget;
using Energy.Domain.BusinessPartners;
using Energy.Domain.Contracts;
using Energy.Domain.Core;
using Energy.Domain.FieldOperations;
using Energy.Domain.Finance;
using Energy.Domain.ProgressPayments;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Budget;

/// <summary>BudgetEntity EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class BudgetConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Budget.Budget>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Budget.Budget> e)
    {
        e.ToTable("Budgets");
        e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<CostCenter>().WithMany().HasForeignKey(x => x.CostCenterId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
    }
}
