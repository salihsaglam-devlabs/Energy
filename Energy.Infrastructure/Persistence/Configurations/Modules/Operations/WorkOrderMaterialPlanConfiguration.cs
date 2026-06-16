using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Operations;

/// <summary>WorkOrderMaterialPlan EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class WorkOrderMaterialPlanConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Operations.WorkOrderMaterialPlan>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Operations.WorkOrderMaterialPlan> builder)
    {
        builder.ToTable("WorkOrderMaterialPlans");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Operations.WorkOrder>().WithMany().HasForeignKey(e => e.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.Material>().WithMany().HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}
