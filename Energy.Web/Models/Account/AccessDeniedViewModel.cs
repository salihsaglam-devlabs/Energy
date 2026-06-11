namespace Energy.Web.Models.Account;

public sealed class AccessDeniedViewModel
{
    public string RequestedPath { get; init; } = "/";

    public string RequestedPermission { get; init; } = "Default.ReadAll";
}

