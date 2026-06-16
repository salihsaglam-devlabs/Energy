using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Assets;

/// <summary>EquipmentMaintenance EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class EquipmentMaintenanceConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Assets.EquipmentMaintenance>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Assets.EquipmentMaintenance> builder)
    {
        builder.ToTable("EquipmentMaintenances");
        builder.HasKey(e => e.Id);
    }
}
