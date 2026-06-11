namespace Energy.Shared.Identity.Permissions;

/// <summary>
/// Compile-time list of every <c>Module.Action</c> permission supported by the
/// application. This is the single source of truth that is mirrored into the
/// <c>permissions</c> table at startup. UI never creates or deletes rows here;
/// adding a new permission is a release-time change.
/// </summary>
public static class PermissionCatalog
{
    public const string DashboardRead = "Dashboard.Read";

    public const string UserRead = "User.Read";
    public const string UserReadAll = "User.ReadAll";
    public const string UserCreate = "User.Create";
    public const string UserUpdate = "User.Update";
    public const string UserDelete = "User.Delete";

    public const string RoleRead = "Role.Read";
    public const string RoleReadAll = "Role.ReadAll";
    public const string RoleCreate = "Role.Create";
    public const string RoleUpdate = "Role.Update";
    public const string RoleDelete = "Role.Delete";

    public const string PermissionRead = "Permission.Read";
    public const string PermissionReadAll = "Permission.ReadAll";

    public const string MenuRead = "Menu.Read";
    public const string MenuReadAll = "Menu.ReadAll";
    public const string MenuCreate = "Menu.Create";
    public const string MenuUpdate = "Menu.Update";
    public const string MenuDelete = "Menu.Delete";

    public const string ApiAccessRead = "ApiAccess.Read";
    public const string ApiAccessReadAll = "ApiAccess.ReadAll";
    public const string ApiAccessCreate = "ApiAccess.Create";
    public const string ApiAccessUpdate = "ApiAccess.Update";
    public const string ApiAccessDelete = "ApiAccess.Delete";

    public const string LocalizationRead = "Localization.Read";
    public const string LocalizationReadAll = "Localization.ReadAll";
    public const string LocalizationCreate = "Localization.Create";
    public const string LocalizationUpdate = "Localization.Update";
    public const string LocalizationDelete = "Localization.Delete";

    public const string LogRead = "Log.Read";
    public const string LogReadAll = "Log.ReadAll";

    public const string SettingRead = "Setting.Read";
    public const string SettingUpdate = "Setting.Update";

    // Self-service: every authenticated user may read and update their own
    // profile. These ship as DEFAULT grants (see <see cref="DefaultGrants"/>).
    public const string ProfileRead = "Profile.Read";
    public const string ProfileUpdate = "Profile.Update";

    // Collaboration: every authenticated user may use the chat. Ships as a
    // DEFAULT grant so all roles can message each other out-of-the-box.
    public const string ChatUse = "Chat.Use";

    /// <summary>Flat list of every declared permission code.</summary>
    public static IReadOnlyList<PermissionDescriptor> All { get; } =
    [
        Describe(DashboardRead),

        Describe(UserRead), Describe(UserReadAll), Describe(UserCreate), Describe(UserUpdate), Describe(UserDelete),

        Describe(RoleRead), Describe(RoleReadAll), Describe(RoleCreate), Describe(RoleUpdate), Describe(RoleDelete),

        Describe(PermissionRead), Describe(PermissionReadAll),

        Describe(MenuRead), Describe(MenuReadAll), Describe(MenuCreate), Describe(MenuUpdate), Describe(MenuDelete),

        Describe(ApiAccessRead), Describe(ApiAccessReadAll), Describe(ApiAccessCreate), Describe(ApiAccessUpdate), Describe(ApiAccessDelete),

        Describe(LocalizationRead), Describe(LocalizationReadAll), Describe(LocalizationCreate), Describe(LocalizationUpdate), Describe(LocalizationDelete),

        Describe(LogRead), Describe(LogReadAll),

        Describe(SettingRead), Describe(SettingUpdate),

        Describe(ProfileRead), Describe(ProfileUpdate),

        Describe(ChatUse),
    ];

    /// <summary>Convenience set for membership checks (O(1)).</summary>
    public static IReadOnlySet<string> AllCodes { get; } =
        new HashSet<string>(All.Select(item => item.Code), StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Permissions every authenticated user must own regardless of role — the
    /// "floor" that requires no explicit assignment. Seeded onto every role so
    /// the dashboard and self-service profile are always reachable.
    /// </summary>
    public static IReadOnlyList<string> DefaultGrants { get; } =
    [
        DashboardRead,
        ProfileRead,
        ProfileUpdate,
        ChatUse,
    ];

    /// <summary>Localization key for the display name.</summary>
    public static string BuildDisplayNameKey(string code) => $"Permissions.{code}.Name";

    /// <summary>Localization key for the description.</summary>
    public static string BuildDescriptionKey(string code) => $"Permissions.{code}.Description";

    private static PermissionDescriptor Describe(string code)
    {
        var parts = code.Split('.', 2);
        if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
        {
            throw new InvalidOperationException($"Permission code '{code}' does not match the required 'Module.Action' format.");
        }

        return new PermissionDescriptor(
            Code: code,
            Module: parts[0],
            Action: parts[1],
            DisplayNameKey: BuildDisplayNameKey(code),
            DescriptionKey: BuildDescriptionKey(code));
    }
}

public readonly record struct PermissionDescriptor(
    string Code,
    string Module,
    string Action,
    string DisplayNameKey,
    string DescriptionKey);

