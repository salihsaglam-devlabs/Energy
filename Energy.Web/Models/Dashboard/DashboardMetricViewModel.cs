namespace Energy.Web.Models.Dashboard;

public sealed class DashboardMetricViewModel
{
    public string LabelKey { get; init; } = string.Empty;
    public string DescriptionKey { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}
