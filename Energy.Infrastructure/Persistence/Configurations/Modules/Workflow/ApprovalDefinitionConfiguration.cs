using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalDefinition EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ApprovalDefinitionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Workflow.ApprovalDefinition>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Workflow.ApprovalDefinition> builder)
    {
        builder.ToTable("ApprovalDefinitions");
        builder.HasKey(e => e.Id);
    }
}
