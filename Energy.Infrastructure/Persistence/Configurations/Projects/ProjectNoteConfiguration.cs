using Energy.Domain.BusinessPartners;
using Energy.Domain.Core;
using Energy.Domain.IAM;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Projects;

/// <summary>ProjectNote EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ProjectNoteConfiguration : IEntityTypeConfiguration<ProjectNote>
{
    public void Configure(EntityTypeBuilder<ProjectNote> e)
    {
        e.ToTable("ProjectNotes");
        e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
    }
}
