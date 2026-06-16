using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalCondition EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ApprovalConditionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Workflow.ApprovalCondition>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Workflow.ApprovalCondition> builder)
    {
        builder.ToTable("ApprovalConditions");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Workflow.ApprovalDefinitionVersion>().WithMany().HasForeignKey(e => e.ApprovalDefinitionVersionId).OnDelete(DeleteBehavior.Restrict);
    }
}
