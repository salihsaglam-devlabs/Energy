using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>WarehouseLocation EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class WarehouseLocationConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Inventory.WarehouseLocation>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Inventory.WarehouseLocation> builder)
    {
        builder.ToTable("WarehouseLocations");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.Warehouse>().WithMany().HasForeignKey(e => e.WarehouseId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.WarehouseLocation>().WithMany().HasForeignKey(e => e.ParentLocationId).OnDelete(DeleteBehavior.Restrict);
    }
}
