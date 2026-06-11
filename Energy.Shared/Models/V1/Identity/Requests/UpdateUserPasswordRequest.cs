namespace Energy.Shared.Models.V1.Identity.Requests;

public sealed class UpdateUserPasswordRequest
{
    public string NewPassword { get; init; } = string.Empty;
}
