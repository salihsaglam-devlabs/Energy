using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Core;

/// <summary>Department EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class DepartmentConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Core.Department>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Core.Department> builder)
    {
        builder.ToTable("Departments");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Core.Company>().WithMany().HasForeignKey(e => e.CompanyId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Core.Department>().WithMany().HasForeignKey(e => e.ParentDepartmentId).OnDelete(DeleteBehavior.Restrict);
    }
}
