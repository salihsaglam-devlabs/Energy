namespace Energy.Web.Models.Dashboard;

public sealed class DashboardQuickLinkViewModel
{
    public string Title { get; init; } = string.Empty;
    public string? Url { get; init; }
    public string? Icon { get; init; }
}
