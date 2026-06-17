using Energy.Domain.Assets;
using Energy.Domain.Catalog;
using Energy.Domain.Contracts;
using Energy.Domain.Core;
using Energy.Domain.FieldOperations;
using Energy.Domain.HR;
using Energy.Domain.IAM;
using Energy.Domain.Inventory;
using Energy.Domain.Operations;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Operations;

/// <summary>WorkOrder EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
{
    public void Configure(EntityTypeBuilder<WorkOrder> e)
    {
        e.ToTable("WorkOrders");
        e.HasIndex(x => x.WorkOrderNo).IsUnique();
        e.HasOne<WorkOrderType>().WithMany().HasForeignKey(x => x.WorkOrderTypeId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<ProjectPhase>().WithMany().HasForeignKey(x => x.ProjectPhaseId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<ProjectLocation>().WithMany().HasForeignKey(x => x.ProjectLocationId).OnDelete(DeleteBehavior.Restrict);
    }
}
