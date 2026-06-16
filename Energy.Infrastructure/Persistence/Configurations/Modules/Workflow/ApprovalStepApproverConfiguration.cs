using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalStepApprover EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ApprovalStepApproverConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Workflow.ApprovalStepApprover>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Workflow.ApprovalStepApprover> builder)
    {
        builder.ToTable("ApprovalStepApprovers");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Workflow.ApprovalStepDefinition>().WithMany().HasForeignKey(e => e.ApprovalStepDefinitionId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.IAM.User>().WithMany().HasForeignKey(e => e.ApproverUserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.IAM.Role>().WithMany().HasForeignKey(e => e.ApproverRoleId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Core.Department>().WithMany().HasForeignKey(e => e.ApproverDepartmentId).OnDelete(DeleteBehavior.Restrict);
    }
}
