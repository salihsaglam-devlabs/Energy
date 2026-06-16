using Energy.Domain.Modules.Documents;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Notifications;
using Energy.Domain.Modules.Reporting;
using Energy.Domain.Modules.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalStepApprover EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ApprovalStepApproverConfiguration : IEntityTypeConfiguration<ApprovalStepApprover>
{
    public void Configure(EntityTypeBuilder<ApprovalStepApprover> e)
    {
        e.ToTable("ApprovalStepApprovers");
        e.HasOne<ApprovalStepDefinition>().WithMany().HasForeignKey(x => x.ApprovalStepDefinitionId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<User>().WithMany().HasForeignKey(x => x.ApproverUserId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Role>().WithMany().HasForeignKey(x => x.ApproverRoleId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Energy.Domain.Modules.Core.Department>().WithMany().HasForeignKey(x => x.ApproverDepartmentId).OnDelete(DeleteBehavior.Restrict);
    }
}
