using Energy.Domain.Modules.Documents;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Notifications;
using Energy.Domain.Modules.Reporting;
using Energy.Domain.Modules.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalRequestApprover EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ApprovalRequestApproverConfiguration : IEntityTypeConfiguration<ApprovalRequestApprover>
{
    public void Configure(EntityTypeBuilder<ApprovalRequestApprover> e)
    {
        e.ToTable("ApprovalRequestApprovers");
        e.HasOne<ApprovalRequestStep>().WithMany().HasForeignKey(x => x.ApprovalRequestStepId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
