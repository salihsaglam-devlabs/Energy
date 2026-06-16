using Energy.Domain.Documents;
using Energy.Domain.IAM;
using Energy.Domain.Notifications;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Workflow;

/// <summary>ApprovalRequest EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ApprovalRequestConfiguration : IEntityTypeConfiguration<ApprovalRequest>
{
    public void Configure(EntityTypeBuilder<ApprovalRequest> e)
    {
        e.ToTable("ApprovalRequests");
        e.HasIndex(x => new { x.RelatedModule, x.RelatedEntityType, x.RelatedEntityId });
        e.HasOne<ApprovalDefinitionVersion>().WithMany().HasForeignKey(x => x.ApprovalDefinitionVersionId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<User>().WithMany().HasForeignKey(x => x.RequestedByUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
