using Energy.Domain.BusinessPartners;
using Energy.Domain.Core;
using Energy.Domain.IAM;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Organization;

/// <summary>EmployeeSkill EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class EmployeeSkillConfiguration : IEntityTypeConfiguration<EmployeeSkill>
{
    public void Configure(EntityTypeBuilder<EmployeeSkill> e)
    {
        e.ToTable("EmployeeSkills"); e.HasIndex(x => x.Code).IsUnique(); 
    }
}
