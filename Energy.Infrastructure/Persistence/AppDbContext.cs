using Energy.Domain.Identity;
using Energy.Domain.Localization;
using Energy.Domain.Logger;
using Energy.Domain.System;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<AccessRule> AccessRules => Set<AccessRule>();
    public DbSet<MenuPermission> MenuPermissions => Set<MenuPermission>();
    public DbSet<AccessRulePermission> AccessRulePermissions => Set<AccessRulePermission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<RoleMenu> RoleMenus => Set<RoleMenu>();
    public DbSet<Log> Logs => Set<Log>();
    public DbSet<Resource> Resources => Set<Resource>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
