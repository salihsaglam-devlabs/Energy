using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Catalog;

/// <summary>MaterialAttributeValue EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class MaterialAttributeValueConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Catalog.MaterialAttributeValue>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Catalog.MaterialAttributeValue> builder)
    {
        builder.ToTable("MaterialAttributeValues");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.Material>().WithMany().HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.MaterialAttributeDefinition>().WithMany().HasForeignKey(e => e.MaterialAttributeDefinitionId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.MaterialAttributeOption>().WithMany().HasForeignKey(e => e.OptionId).OnDelete(DeleteBehavior.Restrict);
    }
}
