using Energy.Domain.Identity;
using Energy.Domain.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class RoleMenuConfiguration : IEntityTypeConfiguration<RoleMenu>
{
    public void Configure(EntityTypeBuilder<RoleMenu> builder)
    {
        builder.ToTable("RoleMenus");
        builder.HasKey(x => new { x.RoleId, x.MenuId }).HasName("PK_RoleMenus");
        builder.HasIndex(x => x.MenuId).HasDatabaseName("IX_RoleMenus_MenuId");
        builder.HasOne<Role>()
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_RoleMenus_Roles_RoleId");
        builder.HasOne<Menu>()
            .WithMany()
            .HasForeignKey(x => x.MenuId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_RoleMenus_Menus_MenuId");
    }
}

