namespace Energy.Shared.Models.V1.Identity.Responses;

public sealed class UserDetailResponse
{
    public Guid Id { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastLoginAt { get; init; }
    public IReadOnlyCollection<RoleSummaryResponse> Roles { get; init; } = Array.Empty<RoleSummaryResponse>();
    public IReadOnlyCollection<string> EffectivePermissions { get; init; } = Array.Empty<string>();
}
