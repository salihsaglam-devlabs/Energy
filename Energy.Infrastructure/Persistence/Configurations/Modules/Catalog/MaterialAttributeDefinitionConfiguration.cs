using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Catalog;

/// <summary>MaterialAttributeDefinition EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class MaterialAttributeDefinitionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Catalog.MaterialAttributeDefinition>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Catalog.MaterialAttributeDefinition> builder)
    {
        builder.ToTable("MaterialAttributeDefinitions");
        builder.HasKey(e => e.Id);
    }
}
