using Energy.Domain.Modules.BusinessPartners;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Organization;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Organization;

/// <summary>ExpenseClaim EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ExpenseClaimConfiguration : IEntityTypeConfiguration<ExpenseClaim>
{
    public void Configure(EntityTypeBuilder<ExpenseClaim> e)
    {
        e.ToTable("ExpenseClaims");
        e.HasIndex(x => x.ClaimNo).IsUnique();
        e.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
    }
}
