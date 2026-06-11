namespace Energy.Shared.Models.V1.Home.Responses;

public sealed class HomeDashboardResponse
{
    public int ActiveUsers { get; init; }

    public int TotalRoles { get; init; }

    public int TotalPermissions { get; init; }

    public int TotalMenus { get; init; }

    public int ReadinessScore { get; init; }

    public int ConfiguredAreaCount { get; init; }

    public IReadOnlyList<HomeQuickLinkResponse> QuickLinks { get; init; } = [];
}
