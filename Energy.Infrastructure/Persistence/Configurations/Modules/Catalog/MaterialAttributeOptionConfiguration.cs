using Energy.Domain.Modules.Catalog;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.Inventory;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Catalog;

/// <summary>MaterialAttributeOption EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class MaterialAttributeOptionConfiguration : IEntityTypeConfiguration<MaterialAttributeOption>
{
    public void Configure(EntityTypeBuilder<MaterialAttributeOption> e)
    {
        e.ToTable("MaterialAttributeOptions");
        e.HasOne<MaterialAttributeDefinition>().WithMany().HasForeignKey(x => x.MaterialAttributeDefinitionId).OnDelete(DeleteBehavior.Cascade);
    }
}
