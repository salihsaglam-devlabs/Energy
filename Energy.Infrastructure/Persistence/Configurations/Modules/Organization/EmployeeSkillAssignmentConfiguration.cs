using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Organization;

/// <summary>EmployeeSkillAssignment EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class EmployeeSkillAssignmentConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Organization.EmployeeSkillAssignment>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Organization.EmployeeSkillAssignment> builder)
    {
        builder.ToTable("EmployeeSkillAssignments");
        builder.HasKey(e => e.Id);
    }
}
