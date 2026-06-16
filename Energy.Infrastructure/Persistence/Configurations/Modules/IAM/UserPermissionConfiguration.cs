using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.IAM;

/// <summary>UserPermission EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class UserPermissionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.IAM.UserPermission>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.IAM.UserPermission> builder)
    {
        builder.ToTable("UserPermissions");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.IAM.User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.IAM.Permission>().WithMany().HasForeignKey(e => e.PermissionCode).HasPrincipalKey("Code").OnDelete(DeleteBehavior.Restrict);
    }
}
