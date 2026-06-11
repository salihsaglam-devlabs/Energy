namespace Energy.Shared.Identity.Permissions;

public static class UserPermissions
{
    public const string GetUsers = "User.GetUsers";
    public const string GetUser = "User.GetUser";
    public const string CreateUser = "User.CreateUser";
    public const string UpdateUser = "User.UpdateUser";
    public const string SetRoles = "User.SetRoles";
    public const string UpdatePassword = "User.UpdatePassword";
    public const string DeleteUser = "User.DeleteUser";
    public const string GetAdminPermissionHealth = "User.GetAdminPermissionHealth";
    public const string GetProfile = "User.GetProfile";
    public const string UpdateProfile = "User.UpdateProfile";

    public static IReadOnlyList<string> All { get; } = new[]
    {
        GetUsers,
        GetUser,
        CreateUser,
        UpdateUser,
        SetRoles,
        UpdatePassword,
        DeleteUser,
        GetAdminPermissionHealth,
        GetProfile,
        UpdateProfile,
    };
}
