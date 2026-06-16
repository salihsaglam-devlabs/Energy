using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.FieldOperations;

/// <summary>ProgressEntry EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ProgressEntryConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.FieldOperations.ProgressEntry>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.FieldOperations.ProgressEntry> builder)
    {
        builder.ToTable("ProgressEntries");
        builder.HasKey(e => e.Id);
    }
}
