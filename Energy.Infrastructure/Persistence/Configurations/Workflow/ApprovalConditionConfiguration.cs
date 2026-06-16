using Energy.Domain.Documents;
using Energy.Domain.IAM;
using Energy.Domain.Notifications;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Workflow;

/// <summary>ApprovalCondition EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ApprovalConditionConfiguration : IEntityTypeConfiguration<ApprovalCondition>
{
    public void Configure(EntityTypeBuilder<ApprovalCondition> e)
    {
        e.ToTable("ApprovalConditions");
        e.HasOne<ApprovalDefinitionVersion>().WithMany().HasForeignKey(x => x.ApprovalDefinitionVersionId).OnDelete(DeleteBehavior.Restrict);
    }
}
