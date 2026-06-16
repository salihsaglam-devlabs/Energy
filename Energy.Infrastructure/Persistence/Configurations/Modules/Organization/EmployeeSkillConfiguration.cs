using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Organization;

/// <summary>EmployeeSkill EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class EmployeeSkillConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Organization.EmployeeSkill>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Organization.EmployeeSkill> builder)
    {
        builder.ToTable("EmployeeSkills");
        builder.HasKey(e => e.Id);
    }
}
