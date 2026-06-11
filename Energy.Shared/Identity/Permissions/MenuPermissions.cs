namespace Energy.Shared.Identity.Permissions;

public static class MenuPermissions
{
    public const string GetMenus = "Menu.GetMenus";
    public const string GetMenuTree = "Menu.GetMenuTree";
    public const string GetMenu = "Menu.GetMenu";
    public const string CreateMenu = "Menu.CreateMenu";
    public const string UpdateMenu = "Menu.UpdateMenu";
    public const string DeleteMenu = "Menu.DeleteMenu";
    public const string GetMenuPermissions = "Menu.GetMenuPermissions";
    public const string SetMenuPermissions = "Menu.SetMenuPermissions";

    public static IReadOnlyList<string> All { get; } = new[]
    {
        GetMenus,
        GetMenuTree,
        GetMenu,
        CreateMenu,
        UpdateMenu,
        DeleteMenu,
        GetMenuPermissions,
        SetMenuPermissions,
    };
}
