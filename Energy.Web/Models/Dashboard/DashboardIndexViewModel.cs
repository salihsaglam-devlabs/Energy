namespace Energy.Web.Models.Dashboard;

public sealed class DashboardIndexViewModel
{
    public int ReadinessScore { get; init; }
    public int ConfiguredAreaCount { get; init; }
    public string StatusKey { get; init; } = string.Empty;
    public IReadOnlyList<DashboardMetricViewModel> Metrics { get; init; } = Array.Empty<DashboardMetricViewModel>();
    public IReadOnlyList<EnterpriseMetricViewModel> EnterpriseMetrics { get; init; } = Array.Empty<EnterpriseMetricViewModel>();
    public IReadOnlyList<DashboardQuickLinkViewModel> QuickLinks { get; init; } = Array.Empty<DashboardQuickLinkViewModel>();
}
