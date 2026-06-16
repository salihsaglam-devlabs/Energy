using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Catalog;

/// <summary>MaterialAttributeOption EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class MaterialAttributeOptionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Catalog.MaterialAttributeOption>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Catalog.MaterialAttributeOption> builder)
    {
        builder.ToTable("MaterialAttributeOptions");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.MaterialAttributeDefinition>().WithMany().HasForeignKey(e => e.MaterialAttributeDefinitionId).OnDelete(DeleteBehavior.Cascade);
    }
}
