using Energy.Domain.BusinessPartners;
using Energy.Domain.Core;
using Energy.Domain.IAM;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Organization;

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
