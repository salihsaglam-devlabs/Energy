using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Projects;

/// <summary>ProjectStatus EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ProjectStatusConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Projects.ProjectStatus>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Projects.ProjectStatus> builder)
    {
        builder.ToTable("ProjectStatuses");
        builder.HasKey(e => e.Id);
    }
}
