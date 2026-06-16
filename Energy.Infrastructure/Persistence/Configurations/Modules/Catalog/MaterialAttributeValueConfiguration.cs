using Energy.Domain.Modules.Catalog;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.Inventory;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Catalog;

/// <summary>MaterialAttributeValue EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class MaterialAttributeValueConfiguration : IEntityTypeConfiguration<MaterialAttributeValue>
{
    public void Configure(EntityTypeBuilder<MaterialAttributeValue> e)
    {
        e.ToTable("MaterialAttributeValues");
        e.HasIndex(x => new { x.MaterialId, x.MaterialAttributeDefinitionId }).IsUnique();
        e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<MaterialAttributeDefinition>().WithMany().HasForeignKey(x => x.MaterialAttributeDefinitionId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<MaterialAttributeOption>().WithMany().HasForeignKey(x => x.OptionId).OnDelete(DeleteBehavior.Restrict);
    }
}
