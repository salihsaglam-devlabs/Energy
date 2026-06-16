using Energy.Domain.Modules.Catalog;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.Inventory;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Catalog;

/// <summary>MaterialCategoryAttribute EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class MaterialCategoryAttributeConfiguration : IEntityTypeConfiguration<MaterialCategoryAttribute>
{
    public void Configure(EntityTypeBuilder<MaterialCategoryAttribute> e)
    {
        e.ToTable("MaterialCategoryAttributes");
        e.HasIndex(x => new { x.MaterialCategoryId, x.MaterialAttributeDefinitionId }).IsUnique();
        e.HasOne<MaterialCategory>().WithMany().HasForeignKey(x => x.MaterialCategoryId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<MaterialAttributeDefinition>().WithMany().HasForeignKey(x => x.MaterialAttributeDefinitionId).OnDelete(DeleteBehavior.Restrict);
    }
}
