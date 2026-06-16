using Energy.Domain.IAM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.ToTable("UserPermissions");
        builder.HasKey(up => new { up.UserId, up.PermissionCode });

        builder.Property(up => up.PermissionCode).HasMaxLength(150);

        builder.HasOne<User>().WithMany().HasForeignKey(up => up.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Permission>().WithMany().HasForeignKey(up => up.PermissionCode).OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(up => up.PermissionCode);
    }
}

