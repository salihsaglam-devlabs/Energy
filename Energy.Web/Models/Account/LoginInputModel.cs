using System.ComponentModel.DataAnnotations;

namespace Energy.Web.Models.Account;

public sealed class LoginInputModel
{
    [Required] public string UserNameOrEmail { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// Geliştirme dışı ortamda hızlı girişi açan URL parametresi; doğrulama hatası
    /// sonrası hızlı girişin görünür kalması için forma gizli alan olarak taşınır.
    /// </summary>
    public string? DevLogin { get; set; }
}
