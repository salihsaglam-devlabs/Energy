using Energy.Domain.Modules.Documents;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Notifications;
using Energy.Domain.Modules.Reporting;
using Energy.Domain.Modules.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalStepDefinition EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ApprovalStepDefinitionConfiguration : IEntityTypeConfiguration<ApprovalStepDefinition>
{
    public void Configure(EntityTypeBuilder<ApprovalStepDefinition> e)
    {
        e.ToTable("ApprovalStepDefinitions");
        e.HasOne<ApprovalDefinitionVersion>().WithMany().HasForeignKey(x => x.ApprovalDefinitionVersionId).OnDelete(DeleteBehavior.Restrict);
    }
}
