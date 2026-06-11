using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Web.Models.Profile;

public sealed class ProfileViewModel
{
    public UserDetailResponse User { get; init; } = new();

    public IReadOnlyList<string> Permissions { get; init; } = Array.Empty<string>();

    public IReadOnlyList<string> RoleKeys { get; init; } = Array.Empty<string>();
}

