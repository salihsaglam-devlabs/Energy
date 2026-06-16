using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalRequestStep EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ApprovalRequestStepConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Workflow.ApprovalRequestStep>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Workflow.ApprovalRequestStep> builder)
    {
        builder.ToTable("ApprovalRequestSteps");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Workflow.ApprovalRequest>().WithMany().HasForeignKey(e => e.ApprovalRequestId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Workflow.ApprovalStepDefinition>().WithMany().HasForeignKey(e => e.ApprovalStepDefinitionId).OnDelete(DeleteBehavior.Restrict);
    }
}
