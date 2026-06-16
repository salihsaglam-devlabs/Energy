using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Operations;

/// <summary>WorkOrderAssignment EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class WorkOrderAssignmentConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Operations.WorkOrderAssignment>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Operations.WorkOrderAssignment> builder)
    {
        builder.ToTable("WorkOrderAssignments");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Operations.WorkOrder>().WithMany().HasForeignKey(e => e.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.Organization.Employee>().WithMany().HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.IAM.User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
