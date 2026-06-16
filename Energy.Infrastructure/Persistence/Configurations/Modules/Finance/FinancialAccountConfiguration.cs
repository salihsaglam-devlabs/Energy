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

/// <summary>FinancialAccount EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class FinancialAccountConfiguration : IEntityTypeConfiguration<FinancialAccount>
{
    public void Configure(EntityTypeBuilder<FinancialAccount> e)
    {
        e.ToTable("FinancialAccounts");
        e.HasIndex(x => x.Code).IsUnique();
        e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
    }
}
