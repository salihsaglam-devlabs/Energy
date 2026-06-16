using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Organization;

/// <summary>Employee EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class EmployeeConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Organization.Employee>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Organization.Employee> builder)
    {
        builder.ToTable("Employees");
        builder.HasKey(e => e.Id);
    }
}
