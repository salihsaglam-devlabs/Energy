namespace Energy.Shared.Models.V1.Identity.Requests;

public sealed class UpdateUserRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public IReadOnlyCollection<Guid> RoleIds { get; set; } = Array.Empty<Guid>();
}

