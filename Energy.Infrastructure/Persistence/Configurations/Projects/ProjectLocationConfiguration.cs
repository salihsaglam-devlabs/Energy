using Energy.Domain.BusinessPartners;
using Energy.Domain.Core;
using Energy.Domain.IAM;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Projects;

/// <summary>ProjectLocation EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ProjectLocationConfiguration : IEntityTypeConfiguration<ProjectLocation>
{
    public void Configure(EntityTypeBuilder<ProjectLocation> e)
    {
        e.ToTable("ProjectLocations");
        e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<ProjectLocation>().WithMany().HasForeignKey(x => x.ParentLocationId).OnDelete(DeleteBehavior.Restrict);
    }
}
