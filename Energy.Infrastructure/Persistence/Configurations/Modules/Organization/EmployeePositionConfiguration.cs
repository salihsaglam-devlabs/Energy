using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Organization;

/// <summary>EmployeePosition EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class EmployeePositionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Organization.EmployeePosition>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Organization.EmployeePosition> builder)
    {
        builder.ToTable("EmployeePositions");
        builder.HasKey(e => e.Id);
    }
}
