using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Projects;

/// <summary>ProjectLocation EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ProjectLocationConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Projects.ProjectLocation>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Projects.ProjectLocation> builder)
    {
        builder.ToTable("ProjectLocations");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Projects.Project>().WithMany().HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Projects.ProjectLocation>().WithMany().HasForeignKey(e => e.ParentLocationId).OnDelete(DeleteBehavior.Restrict);
    }
}
