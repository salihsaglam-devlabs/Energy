using Energy.Domain.Modules.Documents;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Notifications;
using Energy.Domain.Modules.Reporting;
using Energy.Domain.Modules.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalDefinitionVersion EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ApprovalDefinitionVersionConfiguration : IEntityTypeConfiguration<ApprovalDefinitionVersion>
{
    public void Configure(EntityTypeBuilder<ApprovalDefinitionVersion> e)
    {
        e.ToTable("ApprovalDefinitionVersions");
        e.HasIndex(x => new { x.ApprovalDefinitionId, x.VersionNo }).IsUnique();
        e.HasOne<ApprovalDefinition>().WithMany().HasForeignKey(x => x.ApprovalDefinitionId).OnDelete(DeleteBehavior.Restrict);
    }
}
