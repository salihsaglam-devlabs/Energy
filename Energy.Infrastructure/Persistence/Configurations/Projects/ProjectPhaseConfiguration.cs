using Energy.Domain.BusinessPartners;
using Energy.Domain.Core;
using Energy.Domain.IAM;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Projects;

/// <summary>ProjectPhase EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ProjectPhaseConfiguration : IEntityTypeConfiguration<ProjectPhase>
{
    public void Configure(EntityTypeBuilder<ProjectPhase> e)
    {
        e.ToTable("ProjectPhases");
        e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<ProjectPhase>().WithMany().HasForeignKey(x => x.ParentPhaseId).OnDelete(DeleteBehavior.Restrict);
    }
}
