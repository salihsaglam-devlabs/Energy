using Energy.Domain.Modules.BusinessPartners;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Organization;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Organization;

/// <summary>EmployeeSkillAssignment EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class EmployeeSkillAssignmentConfiguration : IEntityTypeConfiguration<EmployeeSkillAssignment>
{
    public void Configure(EntityTypeBuilder<EmployeeSkillAssignment> e)
    {
        e.ToTable("EmployeeSkillAssignments");
        e.HasIndex(x => new { x.EmployeeId, x.EmployeeSkillId }).IsUnique();
        e.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<EmployeeSkill>().WithMany().HasForeignKey(x => x.EmployeeSkillId).OnDelete(DeleteBehavior.Restrict);
    }
}
