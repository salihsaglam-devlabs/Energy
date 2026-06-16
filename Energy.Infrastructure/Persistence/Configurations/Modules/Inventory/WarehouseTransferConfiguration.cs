using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>WarehouseTransfer EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class WarehouseTransferConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Inventory.WarehouseTransfer>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Inventory.WarehouseTransfer> builder)
    {
        builder.ToTable("WarehouseTransfers");
        builder.HasKey(e => e.Id);
    }
}
