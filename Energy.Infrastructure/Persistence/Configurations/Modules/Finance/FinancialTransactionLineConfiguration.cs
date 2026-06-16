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

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Finance;

/// <summary>FinancialTransactionLine EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class FinancialTransactionLineConfiguration : IEntityTypeConfiguration<FinancialTransactionLine>
{
    public void Configure(EntityTypeBuilder<FinancialTransactionLine> e)
    {
        e.ToTable("FinancialTransactionLines");
        e.HasOne<FinancialTransaction>().WithMany().HasForeignKey(x => x.FinancialTransactionId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<CostCenter>().WithMany().HasForeignKey(x => x.CostCenterId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
    }
}
