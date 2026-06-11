namespace Energy.Shared.Models.V1.Identity.Responses;

public sealed class AuthTokenResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public Guid UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;

    /// <summary>Role names the user belongs to (for display / UI grouping).</summary>
    public IReadOnlyList<string> Roles { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Effective permission codes (User → Role → Permission). Drives UI
    /// authorization (menu, page and action gating) on the Web layer.
    /// </summary>
    public IReadOnlyList<string> Permissions { get; init; } = Array.Empty<string>();
}
