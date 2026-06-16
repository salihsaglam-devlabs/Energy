using Energy.Domain.Modules.Budget;
using Energy.Domain.Modules.BusinessPartners;
using Energy.Domain.Modules.Contracts;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.FieldOperations;
using Energy.Domain.Modules.Finance;
using Energy.Domain.Modules.ProgressPayments;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Budget;

/// <summary>BudgetLine EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class BudgetLineConfiguration : IEntityTypeConfiguration<BudgetLine>
{
    public void Configure(EntityTypeBuilder<BudgetLine> e)
    {
        e.ToTable("BudgetLines");
        e.HasOne<global::Energy.Domain.Modules.Budget.Budget>().WithMany().HasForeignKey(x => x.BudgetId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<CostCenter>().WithMany().HasForeignKey(x => x.CostCenterId).OnDelete(DeleteBehavior.Restrict);
    }
}
