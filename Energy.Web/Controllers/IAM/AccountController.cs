using Energy.Localization;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Web.Clients.Identity;
using Energy.Web.Common;
using Energy.Web.Models.Account;
using Energy.Web.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Energy.Web.Controllers.IAM;

[AllowAnonymous]
public sealed class AccountController : Controller
{
    private readonly IAuthApiClient _auth;
    private readonly IAuthCookieFactory _cookies;
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _config;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        IAuthApiClient auth,
        IAuthCookieFactory cookies,
        IWebHostEnvironment env,
        IConfiguration config,
        IStringLocalizer<SharedResource> localizer,
        ILogger<AccountController> logger)
    {
        _auth = auth;
        _cookies = cookies;
        _env = env;
        _config = config;
        _localizer = localizer;
        _logger = logger;
    }

    /// <summary>
    /// Geliştirici hızlı girişinin gösterilip gösterilmeyeceğini belirler.
    /// Geliştirme ortamında her zaman açıktır. Geliştirme dışı (örn. Production)
    /// ortamda yalnızca URL parametresi (<c>?devLogin=...</c>) ile açılabilir:
    ///  - <c>DevLogin:Key</c> yapılandırılmışsa parametre bu gizli anahtara eşit olmalıdır.
    ///  - yapılandırılmamışsa "1" / "true" gibi doğru bir değer yeterlidir.
    /// </summary>
    private bool ShowDevAccounts(string? devLogin)
    {
        if (_env.IsDevelopment())
        {
            return true;
        }

        if (string.IsNullOrWhiteSpace(devLogin))
        {
            return false;
        }

        var key = _config["DevLogin:Key"];
        if (!string.IsNullOrWhiteSpace(key))
        {
            // Sabit zamanlı olmayan basit karşılaştırma yeterli: anahtar yalnızca
            // hızlı giriş listesini açar, kimlik doğrulamayı atlamaz.
            return string.Equals(devLogin, key, StringComparison.Ordinal);
        }

        return devLogin is "1" or "true" or "yes"
            || string.Equals(devLogin, "true", StringComparison.OrdinalIgnoreCase);
    }

    private LoginViewModel BuildLoginModel(string? returnUrl, string? devLogin) => new()
    {
        ReturnUrl = returnUrl,
        // Geliştirmede her zaman; geliştirme dışında yalnızca URL parametresiyle göster.
        DevAccounts = ShowDevAccounts(devLogin) ? DevLoginAccounts.All : Array.Empty<DevAccount>(),
        // Doğrulama hatası sonrası hızlı girişin görünür kalması için parametreyi taşı.
        DevLoginToken = devLogin
    };

    [HttpGet("/account/login")]
    public IActionResult Login(string? returnUrl = null, [FromQuery] string? devLogin = null)
        => View(BuildLoginModel(returnUrl, devLogin));

    [HttpPost("/account/login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginInputModel input, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(BuildLoginModel(input.ReturnUrl, input.DevLogin));

        Shared.Models.V1.Common.Responses.BaseResponse<Shared.Models.V1.Identity.Responses.AuthTokenResponse> response;
        try
        {
            response = await _auth.LoginAsync(new LoginRequest
            {
                UserNameOrEmail = input.UserNameOrEmail,
                Password = input.Password
            }, ct);
        }
        catch (Exception ex)
        {
            // API ile konuşurken oluşan herhangi bir hata (ağ hatası, bir proxy'den
            // gelen JSON olmayan gövde, seri durumdan çıkarma sorunu, istisna olarak
            // yüzeye çıkan 401, ...) global istisna işleyicisine YÜKSELMEMELİDİR — bu,
            // kullanıcıyı giriş ekranında tutmak yerine /Home/Error'a fırlatırdı.
            // Sayfada kal ve anlaşılır bir mesaj göster.
            _logger.LogWarning(ex, "Login API call failed for {User}.", input.UserNameOrEmail);
            ModelState.AddModelError(string.Empty, _localizer[LocalizationKeys.Auth.InvalidCredentials].Value);
            return View(BuildLoginModel(input.ReturnUrl, input.DevLogin));
        }

        if (!response.IsSuccess || response.Data is null)
        {
            // API'nin sağladığı mesajı tercih et, ancak kullanıcının girişin neden
            // reddedildiğini görmesi için her zaman boş olmayan, yerelleştirilmiş bir
            // uyarı garanti et.
            var message = string.IsNullOrWhiteSpace(response.Message)
                ? _localizer[LocalizationKeys.Auth.InvalidCredentials].Value
                : response.Message;
            ModelState.AddModelError(string.Empty, message);
            return View(BuildLoginModel(input.ReturnUrl, input.DevLogin));
        }

        await _cookies.SignInAsync(HttpContext, response.Data);
        // Açık yönlendirme (open-redirect) istismarını önlemek için kullanıcının
        // sağladığı returnUrl'i doğrula: yalnızca aynı siteye ait yerel yollar kabul
        // edilir, aksi hâlde köke geri dönülür.
        return Redirect(Url.GetLocalReturnUrl(input.ReturnUrl, "/"));
    }

    [HttpGet("/account/logout"), HttpPost("/account/logout")]
    public async Task<IActionResult> Logout()
    {
        await _cookies.SignOutAsync(HttpContext);
        return Redirect("/account/login");
    }

    [HttpGet("/account/access-denied")]
    public IActionResult AccessDenied(string? path = null, string? permission = null)
        => View(new AccessDeniedViewModel
        {
            RequestedPath = string.IsNullOrWhiteSpace(path) ? "/" : path,
            // Gerçek gereken yetkiyi olduğu gibi geçir; hiçbiri verilmediyse katalog dışı
            // bir kod uydurmak yerine null bırak.
            RequestedPermission = string.IsNullOrWhiteSpace(permission) ? null : permission
        });
}
