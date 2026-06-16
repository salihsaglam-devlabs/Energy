using Energy.Domain.Modules.Catalog;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.Inventory;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Catalog;

/// <summary>MaterialAttributeDefinition EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class MaterialAttributeDefinitionConfiguration : IEntityTypeConfiguration<MaterialAttributeDefinition>
{
    public void Configure(EntityTypeBuilder<MaterialAttributeDefinition> e)
    {
        e.ToTable("MaterialAttributeDefinitions"); e.HasIndex(x => x.Code).IsUnique(); 
    }
}
