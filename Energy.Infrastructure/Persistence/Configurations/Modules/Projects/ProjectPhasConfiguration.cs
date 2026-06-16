using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Projects;

/// <summary>ProjectPhas EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ProjectPhasConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Projects.ProjectPhas>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Projects.ProjectPhas> builder)
    {
        builder.ToTable("ProjectPhases");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Projects.Project>().WithMany().HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Projects.ProjectPhas>().WithMany().HasForeignKey(e => e.ParentPhaseId).OnDelete(DeleteBehavior.Restrict);
    }
}
