using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Assets;

/// <summary>EquipmentAssignment EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class EquipmentAssignmentConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Assets.EquipmentAssignment>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Assets.EquipmentAssignment> builder)
    {
        builder.ToTable("EquipmentAssignments");
        builder.HasKey(e => e.Id);
    }
}
