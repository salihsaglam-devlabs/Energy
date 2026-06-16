using Energy.Domain.Modules.Documents;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Notifications;
using Energy.Domain.Modules.Reporting;
using Energy.Domain.Modules.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalRequestStep EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ApprovalRequestStepConfiguration : IEntityTypeConfiguration<ApprovalRequestStep>
{
    public void Configure(EntityTypeBuilder<ApprovalRequestStep> e)
    {
        e.ToTable("ApprovalRequestSteps");
        e.HasOne<ApprovalRequest>().WithMany().HasForeignKey(x => x.ApprovalRequestId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<ApprovalStepDefinition>().WithMany().HasForeignKey(x => x.ApprovalStepDefinitionId).OnDelete(DeleteBehavior.Restrict);
    }
}
