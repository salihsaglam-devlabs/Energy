using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Operations;

/// <summary>WorkOrderStatusHistory EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class WorkOrderStatusHistoryConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Operations.WorkOrderStatusHistory>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Operations.WorkOrderStatusHistory> builder)
    {
        builder.ToTable("WorkOrderStatusHistories");
        builder.HasKey(e => e.Id);
    }
}
