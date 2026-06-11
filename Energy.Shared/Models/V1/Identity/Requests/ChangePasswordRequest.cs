namespace Energy.Shared.Models.V1.Identity.Requests;

public sealed class ChangePasswordRequest
{
    public string NewPassword { get; set; } = string.Empty;
}

