using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalStepDefinition EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ApprovalStepDefinitionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Workflow.ApprovalStepDefinition>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Workflow.ApprovalStepDefinition> builder)
    {
        builder.ToTable("ApprovalStepDefinitions");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Workflow.ApprovalDefinitionVersion>().WithMany().HasForeignKey(e => e.ApprovalDefinitionVersionId).OnDelete(DeleteBehavior.Restrict);
    }
}
