namespace Energy.Shared.Identity.Permissions;

public static class PermissionPermissions
{
    public const string GetPermissions = "Permission.GetPermissions";
    public const string GetPermission = "Permission.GetPermission";
    public const string CreatePermission = "Permission.CreatePermission";
    public const string UpdatePermission = "Permission.UpdatePermission";
    public const string DeletePermission = "Permission.DeletePermission";

    public static IReadOnlyList<string> All { get; } = new[]
    {
        GetPermissions,
        GetPermission,
        CreatePermission,
        UpdatePermission,
        DeletePermission,
    };
}
