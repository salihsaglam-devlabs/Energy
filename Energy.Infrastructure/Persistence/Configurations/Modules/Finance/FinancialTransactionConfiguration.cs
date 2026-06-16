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

/// <summary>FinancialTransaction EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class FinancialTransactionConfiguration : IEntityTypeConfiguration<FinancialTransaction>
{
    public void Configure(EntityTypeBuilder<FinancialTransaction> e)
    {
        e.ToTable("FinancialTransactions");
        e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.PartnerId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<FinancialAccount>().WithMany().HasForeignKey(x => x.FinancialAccountId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<CostCenter>().WithMany().HasForeignKey(x => x.CostCenterId).OnDelete(DeleteBehavior.Restrict);
    }
}
