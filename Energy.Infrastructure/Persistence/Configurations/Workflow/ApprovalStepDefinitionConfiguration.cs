using Energy.Domain.Documents;
using Energy.Domain.IAM;
using Energy.Domain.Notifications;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Workflow;

/// <summary>ApprovalStepDefinition EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ApprovalStepDefinitionConfiguration : IEntityTypeConfiguration<ApprovalStepDefinition>
{
    public void Configure(EntityTypeBuilder<ApprovalStepDefinition> e)
    {
        e.ToTable("ApprovalStepDefinitions");
        e.HasOne<ApprovalDefinitionVersion>().WithMany().HasForeignKey(x => x.ApprovalDefinitionVersionId).OnDelete(DeleteBehavior.Restrict);
    }
}
