namespace Energy.Shared.Models.V1.Identity.Responses;

public sealed class UserSummaryResponse
{
    public Guid Id { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime? LastLoginAt { get; init; }
    public IReadOnlyCollection<string> RoleNames { get; init; } = Array.Empty<string>();
}
