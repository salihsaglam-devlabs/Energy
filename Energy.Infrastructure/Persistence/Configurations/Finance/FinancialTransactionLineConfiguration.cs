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

namespace Energy.Infrastructure.Persistence.Configurations.Finance;

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
