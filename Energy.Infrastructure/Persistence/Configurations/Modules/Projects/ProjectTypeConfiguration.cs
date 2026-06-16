using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Projects;

/// <summary>ProjectType EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ProjectTypeConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Projects.ProjectType>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Projects.ProjectType> builder)
    {
        builder.ToTable("ProjectTypes");
        builder.HasKey(e => e.Id);
    }
}
