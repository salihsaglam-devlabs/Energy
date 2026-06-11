namespace Energy.Shared.Models.V1.Home.Requests;

public sealed class GetHomeDashboardRequest
{
    public bool IncludeQuickLinks { get; init; } = true;

    public int QuickLinkCount { get; init; } = 4;
}
