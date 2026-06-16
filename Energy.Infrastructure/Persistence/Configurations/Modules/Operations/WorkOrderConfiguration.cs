using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Operations;

/// <summary>WorkOrder EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class WorkOrderConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Operations.WorkOrder>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Operations.WorkOrder> builder)
    {
        builder.ToTable("WorkOrders");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Operations.WorkOrderType>().WithMany().HasForeignKey(e => e.WorkOrderTypeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Projects.Project>().WithMany().HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Projects.ProjectPhas>().WithMany().HasForeignKey(e => e.ProjectPhaseId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Projects.ProjectLocation>().WithMany().HasForeignKey(e => e.ProjectLocationId).OnDelete(DeleteBehavior.Restrict);
    }
}
