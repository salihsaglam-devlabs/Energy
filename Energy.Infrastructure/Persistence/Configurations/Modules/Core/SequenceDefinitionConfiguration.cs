using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Core;

/// <summary>SequenceDefinition EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class SequenceDefinitionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Core.SequenceDefinition>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Core.SequenceDefinition> builder)
    {
        builder.ToTable("SequenceDefinitions");
        builder.HasKey(e => e.Id);
    }
}
