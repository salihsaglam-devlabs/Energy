using Energy.Domain.Documents;
using Energy.Domain.IAM;
using Energy.Domain.Notifications;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Workflow;

/// <summary>ApprovalStepApprover EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ApprovalStepApproverConfiguration : IEntityTypeConfiguration<ApprovalStepApprover>
{
    public void Configure(EntityTypeBuilder<ApprovalStepApprover> e)
    {
        e.ToTable("ApprovalStepApprovers");
        e.HasOne<ApprovalStepDefinition>().WithMany().HasForeignKey(x => x.ApprovalStepDefinitionId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<User>().WithMany().HasForeignKey(x => x.ApproverUserId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Role>().WithMany().HasForeignKey(x => x.ApproverRoleId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Energy.Domain.Core.Department>().WithMany().HasForeignKey(x => x.ApproverDepartmentId).OnDelete(DeleteBehavior.Restrict);
    }
}
