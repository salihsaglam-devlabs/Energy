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

/// <summary>WorkOrderChecklist EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class WorkOrderChecklistConfiguration : IEntityTypeConfiguration<WorkOrderChecklist>
{
    public void Configure(EntityTypeBuilder<WorkOrderChecklist> e)
    {
        e.ToTable("WorkOrderChecklists");
        e.HasOne<WorkOrder>().WithMany().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
    }
}
