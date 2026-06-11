namespace Energy.Shared.Identity.Permissions;

/// <summary>
/// Canonical action verbs used by every <c>Module.Action</c> permission code.
/// </summary>
public static class PermissionActions
{
    public const string Read = "Read";
    public const string ReadAll = "ReadAll";
    public const string Create = "Create";
    public const string Update = "Update";
    public const string Delete = "Delete";
}

/// <summary>
/// Canonical module names. New modules must follow the same PascalCase form.
/// </summary>
public static class PermissionModules
{
    public const string Dashboard = "Dashboard";
    public const string User = "User";
    public const string Role = "Role";
    public const string Permission = "Permission";
    public const string Menu = "Menu";
    public const string ApiAccess = "ApiAccess";
    public const string Localization = "Localization";
    public const string Log = "Log";
    public const string Setting = "Setting";
    public const string Profile = "Profile";
}

