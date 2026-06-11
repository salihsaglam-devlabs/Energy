namespace Energy.Web.Models.Account;

/// <summary>
/// Posted by the DevExtreme login form.
/// </summary>
public sealed class LoginInputModel
{
    public string UserNameOrEmail { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}

