using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Projects;

/// <summary>ProjectMember EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ProjectMemberConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Projects.ProjectMember>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Projects.ProjectMember> builder)
    {
        builder.ToTable("ProjectMembers");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Projects.Project>().WithMany().HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.IAM.User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Organization.Employee>().WithMany().HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}
