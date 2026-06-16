using Energy.Domain.BusinessPartners;
using Energy.Domain.Core;
using Energy.Domain.IAM;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Organization;

/// <summary>ExpenseClaimLine EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ExpenseClaimLineConfiguration : IEntityTypeConfiguration<ExpenseClaimLine>
{
    public void Configure(EntityTypeBuilder<ExpenseClaimLine> e)
    {
        e.ToTable("ExpenseClaimLines");
        e.HasOne<ExpenseClaim>().WithMany().HasForeignKey(x => x.ExpenseClaimId).OnDelete(DeleteBehavior.Cascade);
    }
}
