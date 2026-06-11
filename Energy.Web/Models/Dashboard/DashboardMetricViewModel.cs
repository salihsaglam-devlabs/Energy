namespace Energy.Web.Models.Dashboard;

public sealed class DashboardMetricViewModel
{
    public required string LabelKey { get; init; }

    public required string DescriptionKey { get; init; }

    public required string Value { get; init; }
}

