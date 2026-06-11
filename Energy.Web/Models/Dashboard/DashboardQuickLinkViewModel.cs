namespace Energy.Web.Models.Dashboard;

public sealed class DashboardQuickLinkViewModel
{
    public required string Title { get; init; }

    public required string Url { get; init; }

    public string? Icon { get; init; }
}

