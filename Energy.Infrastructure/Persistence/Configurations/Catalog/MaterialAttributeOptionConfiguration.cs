using Energy.Domain.Catalog;
using Energy.Domain.Core;
using Energy.Domain.Inventory;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Catalog;

/// <summary>MaterialAttributeOption EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class MaterialAttributeOptionConfiguration : IEntityTypeConfiguration<MaterialAttributeOption>
{
    public void Configure(EntityTypeBuilder<MaterialAttributeOption> e)
    {
        e.ToTable("MaterialAttributeOptions");
        e.HasOne<MaterialAttributeDefinition>().WithMany().HasForeignKey(x => x.MaterialAttributeDefinitionId).OnDelete(DeleteBehavior.Cascade);
    }
}
