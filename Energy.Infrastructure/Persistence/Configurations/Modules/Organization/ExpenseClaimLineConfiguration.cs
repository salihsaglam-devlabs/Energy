using Energy.Domain.Modules.BusinessPartners;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Organization;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Organization;

/// <summary>ExpenseClaimLine EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ExpenseClaimLineConfiguration : IEntityTypeConfiguration<ExpenseClaimLine>
{
    public void Configure(EntityTypeBuilder<ExpenseClaimLine> e)
    {
        e.ToTable("ExpenseClaimLines");
        e.HasOne<ExpenseClaim>().WithMany().HasForeignKey(x => x.ExpenseClaimId).OnDelete(DeleteBehavior.Cascade);
    }
}
