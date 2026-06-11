using Energy.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");
        builder.HasKey(userRole => new { userRole.UserId, userRole.RoleId }).HasName("PK_UserRoles");
        builder.HasIndex(userRole => userRole.RoleId).HasDatabaseName("IX_UserRoles_RoleId");
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(userRole => userRole.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_UserRoles_Users_UserId");
        builder.HasOne<Role>()
            .WithMany()
            .HasForeignKey(userRole => userRole.RoleId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_UserRoles_Roles_RoleId");
    }
}
