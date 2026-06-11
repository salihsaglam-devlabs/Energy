namespace Energy.Shared.Models.V1.Identity.Responses;

public sealed class AuthTokenResponse
{
    public string AccessToken { get; init; } = string.Empty;

    public string TokenType { get; init; } = "Bearer";

    public DateTime ExpiresAt { get; init; }

    public Guid UserId { get; init; }

    public string? UserName { get; init; }

    public string? Email { get; init; }

    public IReadOnlyList<string> Roles { get; init; } = [];

    /// <summary>
    /// Stable, culture-independent identifiers for the user's roles
    /// (the role's <c>NormalizedName</c>). Used by clients to detect
    /// well-known roles (e.g. Admin) regardless of how their display name
    /// is localized.
    /// </summary>
    public IReadOnlyList<string> RoleKeys { get; init; } = [];

    public IReadOnlyList<string> Permissions { get; init; } = [];
}

