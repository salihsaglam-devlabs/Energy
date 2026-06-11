namespace Energy.Web.Models.Dashboard;

public sealed class DashboardIndexViewModel
{
    public required int ReadinessScore { get; init; }

    public required int ConfiguredAreaCount { get; init; }

    public required string StatusKey { get; init; }

    public required IReadOnlyList<DashboardMetricViewModel> Metrics { get; init; }

    public required IReadOnlyList<DashboardQuickLinkViewModel> QuickLinks { get; init; }
}

