using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.IAM;

/// <summary>RolePermission EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class RolePermissionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.IAM.RolePermission>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.IAM.RolePermission> builder)
    {
        builder.ToTable("RolePermissions");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.IAM.Role>().WithMany().HasForeignKey(e => e.RoleId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.IAM.Permission>().WithMany().HasForeignKey(e => e.PermissionCode).HasPrincipalKey("Code").OnDelete(DeleteBehavior.Restrict);
    }
}
