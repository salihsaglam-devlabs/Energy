using Energy.Domain.Assets;
using Energy.Domain.Catalog;
using Energy.Domain.Contracts;
using Energy.Domain.Core;
using Energy.Domain.FieldOperations;
using Energy.Domain.HR;
using Energy.Domain.IAM;
using Energy.Domain.Inventory;
using Energy.Domain.Operations;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.FieldOperations;

/// <summary>ProgressEntry EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ProgressEntryConfiguration : IEntityTypeConfiguration<ProgressEntry>
{
    public void Configure(EntityTypeBuilder<ProgressEntry> e)
    {
        e.ToTable("ProgressEntries");
        e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<ProjectPhase>().WithMany().HasForeignKey(x => x.ProjectPhaseId).OnDelete(DeleteBehavior.Restrict);
    }
}
