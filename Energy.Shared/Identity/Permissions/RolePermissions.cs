namespace Energy.Shared.Identity.Permissions;

public static class RolePermissions
{
    public const string GetRoles = "Role.GetRoles";
    public const string GetRole = "Role.GetRole";
    public const string CreateRole = "Role.CreateRole";
    public const string UpdateRole = "Role.UpdateRole";
    public const string DeleteRole = "Role.DeleteRole";
    public const string GetRolePermissions = "Role.GetRolePermissions";
    public const string SetRolePermissions = "Role.SetRolePermissions";
    public const string GetRoleMenus = "Role.GetRoleMenus";
    public const string SetRoleMenus = "Role.SetRoleMenus";

    public static IReadOnlyList<string> All { get; } = new[]
    {
        GetRoles,
        GetRole,
        CreateRole,
        UpdateRole,
        DeleteRole,
        GetRolePermissions,
        SetRolePermissions,
        GetRoleMenus,
        SetRoleMenus,
    };
}
