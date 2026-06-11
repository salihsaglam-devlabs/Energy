namespace Energy.Shared.Models.V1.Identity.Requests;

public sealed class LoginRequest
{
    public string UserNameOrEmail { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}

