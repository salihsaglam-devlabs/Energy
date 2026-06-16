using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Operations;

/// <summary>WorkOrderChecklist EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class WorkOrderChecklistConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Operations.WorkOrderChecklist>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Operations.WorkOrderChecklist> builder)
    {
        builder.ToTable("WorkOrderChecklists");
        builder.HasKey(e => e.Id);
    }
}
