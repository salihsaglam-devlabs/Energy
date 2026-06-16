using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Assets;

/// <summary>EquipmentAsset EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class EquipmentAssetConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Assets.EquipmentAsset>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Assets.EquipmentAsset> builder)
    {
        builder.ToTable("EquipmentAssets");
        builder.HasKey(e => e.Id);
    }
}
