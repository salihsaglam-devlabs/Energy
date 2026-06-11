namespace Energy.Shared.Models.V1.Identity.Requests;

public sealed class ValidateCredentialsRequest
{
    public string UserNameOrEmail { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}
