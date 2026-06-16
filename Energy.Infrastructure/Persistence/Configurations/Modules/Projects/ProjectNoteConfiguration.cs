using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Projects;

/// <summary>ProjectNote EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ProjectNoteConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Projects.ProjectNote>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Projects.ProjectNote> builder)
    {
        builder.ToTable("ProjectNotes");
        builder.HasKey(e => e.Id);
    }
}
