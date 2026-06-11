namespace Energy.Shared.Identity.Permissions;

public static class HomePermissions
{
    public const string GetDashboard = "Home.GetDashboard";

    public static IReadOnlyList<string> All { get; } = new[]
    {
        GetDashboard,
    };
}
