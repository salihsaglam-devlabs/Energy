using Energy.Domain.Identity;
using Energy.Domain.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class MenuPermissionConfiguration : IEntityTypeConfiguration<MenuPermission>
{
    public void Configure(EntityTypeBuilder<MenuPermission> builder)
    {
        builder.ToTable("MenuPermissions");
        builder.HasKey(x => new { x.MenuId, x.PermissionId }).HasName("PK_MenuPermissions");
        builder.HasIndex(x => x.PermissionId).HasDatabaseName("IX_MenuPermissions_PermissionId");
        builder.HasOne<Menu>()
            .WithMany()
            .HasForeignKey(x => x.MenuId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_MenuPermissions_Menus_MenuId");
        builder.HasOne<Permission>()
            .WithMany()
            .HasForeignKey(x => x.PermissionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_MenuPermissions_Permissions_PermissionId");
    }
}

