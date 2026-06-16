using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalDefinitionVersion EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ApprovalDefinitionVersionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Workflow.ApprovalDefinitionVersion>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Workflow.ApprovalDefinitionVersion> builder)
    {
        builder.ToTable("ApprovalDefinitionVersions");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Workflow.ApprovalDefinition>().WithMany().HasForeignKey(e => e.ApprovalDefinitionId).OnDelete(DeleteBehavior.Restrict);
    }
}
