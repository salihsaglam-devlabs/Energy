using Energy.Domain.BusinessPartners;
using Energy.Domain.Core;
using Energy.Domain.IAM;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Projects;

/// <summary>ProjectType EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ProjectTypeConfiguration : IEntityTypeConfiguration<ProjectType>
{
    public void Configure(EntityTypeBuilder<ProjectType> e)
    {
        e.ToTable("ProjectTypes"); e.HasIndex(x => x.Code).IsUnique(); 
    }
}
