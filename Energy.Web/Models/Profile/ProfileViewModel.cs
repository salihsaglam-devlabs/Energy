namespace Energy.Web.Models.Profile;

/// <summary>
/// Read-only summary used by the Profile screen. Populated from the cookie
/// principal's claims plus a best-effort lookup to <c>/users/{id}</c>.
/// </summary>
public sealed class ProfileViewModel
{
    public Guid UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public bool IsActive { get; init; } = true;
    public bool HasProfileImage { get; init; }
    public IReadOnlyList<ProfileRoleViewModel> Roles { get; init; } = Array.Empty<ProfileRoleViewModel>();
    public IReadOnlyList<string> Permissions { get; init; } = Array.Empty<string>();
}

public sealed class ProfileRoleViewModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}
