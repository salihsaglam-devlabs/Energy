using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Projects;

/// <summary>Project EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ProjectConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Projects.Project>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Projects.Project> builder)
    {
        builder.ToTable("Projects");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Core.Company>().WithMany().HasForeignKey(e => e.CompanyId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Core.Branch>().WithMany().HasForeignKey(e => e.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Projects.ProjectType>().WithMany().HasForeignKey(e => e.ProjectTypeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Projects.ProjectStatus>().WithMany().HasForeignKey(e => e.StatusId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.BusinessPartners.BusinessPartner>().WithMany().HasForeignKey(e => e.CustomerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.IAM.User>().WithMany().HasForeignKey(e => e.ManagerUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
