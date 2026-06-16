using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalDelegation EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ApprovalDelegationConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Workflow.ApprovalDelegation>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Workflow.ApprovalDelegation> builder)
    {
        builder.ToTable("ApprovalDelegations");
        builder.HasKey(e => e.Id);
    }
}
