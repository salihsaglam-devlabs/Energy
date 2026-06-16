using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Operations;

/// <summary>WorkOrderChecklistItem EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class WorkOrderChecklistItemConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Operations.WorkOrderChecklistItem>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Operations.WorkOrderChecklistItem> builder)
    {
        builder.ToTable("WorkOrderChecklistItems");
        builder.HasKey(e => e.Id);
    }
}
