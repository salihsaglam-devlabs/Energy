using System.ComponentModel.DataAnnotations;

namespace Energy.Web.Models.Account;

public sealed class LoginInputModel
{
    [Required] public string UserNameOrEmail { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    public string? ReturnUrl { get; set; }
}
