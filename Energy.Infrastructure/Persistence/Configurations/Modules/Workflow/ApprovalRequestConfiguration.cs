using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalRequest EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ApprovalRequestConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Workflow.ApprovalRequest>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Workflow.ApprovalRequest> builder)
    {
        builder.ToTable("ApprovalRequests");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Workflow.ApprovalDefinitionVersion>().WithMany().HasForeignKey(e => e.ApprovalDefinitionVersionId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.IAM.User>().WithMany().HasForeignKey(e => e.RequestedByUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
