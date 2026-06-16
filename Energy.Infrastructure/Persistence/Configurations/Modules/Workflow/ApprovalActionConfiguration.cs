using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalAction EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ApprovalActionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Workflow.ApprovalAction>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Workflow.ApprovalAction> builder)
    {
        builder.ToTable("ApprovalActions");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Workflow.ApprovalRequest>().WithMany().HasForeignKey(e => e.ApprovalRequestId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Workflow.ApprovalRequestStep>().WithMany().HasForeignKey(e => e.ApprovalRequestStepId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.IAM.User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
