namespace Energy.Shared.Models.V1.Identity.Requests;

public sealed class LoginRequest
{
    public string UserNameOrEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

