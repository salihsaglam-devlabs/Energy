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

/// <summary>CostCenter EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class CostCenterConfiguration : IEntityTypeConfiguration<CostCenter>
{
    public void Configure(EntityTypeBuilder<CostCenter> e)
    {
        e.ToTable("CostCenters");
        e.HasIndex(x => x.Code).IsUnique();
        e.HasOne<CostCenter>().WithMany().HasForeignKey(x => x.ParentCostCenterId).OnDelete(DeleteBehavior.Restrict);
    }
}
