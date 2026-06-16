using Energy.Domain.Catalog;
using Energy.Domain.Core;
using Energy.Domain.Inventory;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Catalog;

/// <summary>MaterialAttributeDefinition EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class MaterialAttributeDefinitionConfiguration : IEntityTypeConfiguration<MaterialAttributeDefinition>
{
    public void Configure(EntityTypeBuilder<MaterialAttributeDefinition> e)
    {
        e.ToTable("MaterialAttributeDefinitions"); e.HasIndex(x => x.Code).IsUnique(); 
    }
}
