using Energy.Domain.Documents;
using Energy.Domain.IAM;
using Energy.Domain.Notifications;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Workflow;

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
