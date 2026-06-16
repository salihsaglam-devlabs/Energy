using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.IAM;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Core;

/// <summary>SequenceDefinition EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class SequenceDefinitionConfiguration : IEntityTypeConfiguration<SequenceDefinition>
{
    public void Configure(EntityTypeBuilder<SequenceDefinition> e)
    {
        e.ToTable("SequenceDefinitions");
        e.HasIndex(x => new { x.Module, x.EntityType }).IsUnique();
    }
}
