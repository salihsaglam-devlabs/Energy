using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalRequestApprover EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ApprovalRequestApproverConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Workflow.ApprovalRequestApprover>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Workflow.ApprovalRequestApprover> builder)
    {
        builder.ToTable("ApprovalRequestApprovers");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Workflow.ApprovalRequestStep>().WithMany().HasForeignKey(e => e.ApprovalRequestStepId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.IAM.User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
