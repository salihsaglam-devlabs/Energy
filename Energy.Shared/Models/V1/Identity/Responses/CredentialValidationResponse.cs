namespace Energy.Shared.Models.V1.Identity.Responses;

public sealed class CredentialValidationResponse
{
    public bool IsAuthenticated { get; init; }

    public Guid? UserId { get; init; }

    public string? UserName { get; init; }

    public string? Email { get; init; }

    public bool IsActive { get; init; }

    public bool IsLockedOut { get; init; }

    public IReadOnlyList<string> Roles { get; init; } = [];

    /// <summary>
    /// Stable, culture-independent identifiers for the user's roles
    /// (the role's <c>NormalizedName</c>).
    /// </summary>
    public IReadOnlyList<string> RoleKeys { get; init; } = [];
}
