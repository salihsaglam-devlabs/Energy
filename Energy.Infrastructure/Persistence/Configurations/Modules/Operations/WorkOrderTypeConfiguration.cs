using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Operations;

/// <summary>WorkOrderType EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class WorkOrderTypeConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Operations.WorkOrderType>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Operations.WorkOrderType> builder)
    {
        builder.ToTable("WorkOrderTypes");
        builder.HasKey(e => e.Id);
    }
}
