using Energy.Domain.Modules.Documents;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Notifications;
using Energy.Domain.Modules.Reporting;
using Energy.Domain.Modules.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalCondition EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ApprovalConditionConfiguration : IEntityTypeConfiguration<ApprovalCondition>
{
    public void Configure(EntityTypeBuilder<ApprovalCondition> e)
    {
        e.ToTable("ApprovalConditions");
        e.HasOne<ApprovalDefinitionVersion>().WithMany().HasForeignKey(x => x.ApprovalDefinitionVersionId).OnDelete(DeleteBehavior.Restrict);
    }
}
