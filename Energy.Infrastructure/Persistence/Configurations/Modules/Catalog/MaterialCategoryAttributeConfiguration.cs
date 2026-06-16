using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Catalog;

/// <summary>MaterialCategoryAttribute EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class MaterialCategoryAttributeConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Catalog.MaterialCategoryAttribute>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Catalog.MaterialCategoryAttribute> builder)
    {
        builder.ToTable("MaterialCategoryAttributes");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.MaterialCategory>().WithMany().HasForeignKey(e => e.MaterialCategoryId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.MaterialAttributeDefinition>().WithMany().HasForeignKey(e => e.MaterialAttributeDefinitionId).OnDelete(DeleteBehavior.Restrict);
    }
}
