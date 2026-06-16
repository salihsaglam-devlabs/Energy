using Energy.Domain.Modules.Assets;
using Energy.Domain.Modules.Catalog;
using Energy.Domain.Modules.Contracts;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.FieldOperations;
using Energy.Domain.Modules.HR;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Inventory;
using Energy.Domain.Modules.Operations;
using Energy.Domain.Modules.Organization;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Operations;

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
