using Energy.Localization;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Web.Clients.Identity;
using Energy.Web.Common;
using Energy.Web.Models.Account;
using Energy.Web.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Energy.Web.Controllers;

[AllowAnonymous]
public sealed class AccountController : Controller
{
    private readonly IAuthApiClient _auth;
    private readonly IAuthCookieFactory _cookies;
    private readonly IWebHostEnvironment _env;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        IAuthApiClient auth,
        IAuthCookieFactory cookies,
        IWebHostEnvironment env,
        IStringLocalizer<SharedResource> localizer,
        ILogger<AccountController> logger)
    {
        _auth = auth;
        _cookies = cookies;
        _env = env;
        _localizer = localizer;
        _logger = logger;
    }

    private LoginViewModel BuildLoginModel(string? returnUrl) => new()
    {
        ReturnUrl = returnUrl,
        // Only expose the seeded quick-login presets while developing.
        DevAccounts = _env.IsDevelopment() ? DevLoginAccounts.All : Array.Empty<DevAccount>()
    };

    [HttpGet("/account/login")]
    public IActionResult Login(string? returnUrl = null)
        => View(BuildLoginModel(returnUrl));

    [HttpPost("/account/login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginInputModel input, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(BuildLoginModel(input.ReturnUrl));

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
            // Any failure talking to the API (network error, non-JSON body from a
            // proxy, deserialization issue, 401 surfaced as an exception, ...) must
            // NOT bubble up to the global exception handler — that would bounce the
            // user to /Home/Error instead of keeping them on the login screen. Stay
            // on the page and show a friendly message.
            _logger.LogWarning(ex, "Login API call failed for {User}.", input.UserNameOrEmail);
            ModelState.AddModelError(string.Empty, _localizer[LocalizationKeys.Auth.InvalidCredentials].Value);
            return View(BuildLoginModel(input.ReturnUrl));
        }

        if (!response.IsSuccess || response.Data is null)
        {
            // Prefer the API-provided message, but always guarantee a non-empty,
            // localized warning so the user sees why the login was rejected.
            var message = string.IsNullOrWhiteSpace(response.Message)
                ? _localizer[LocalizationKeys.Auth.InvalidCredentials].Value
                : response.Message;
            ModelState.AddModelError(string.Empty, message);
            return View(BuildLoginModel(input.ReturnUrl));
        }

        await _cookies.SignInAsync(HttpContext, response.Data);
        // Validate the user-supplied returnUrl to avoid open-redirect abuse:
        // only same-site local paths are honoured, otherwise fall back to root.
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
            RequestedPermission = string.IsNullOrWhiteSpace(permission) ? "Default.ReadAll" : permission
        });
}
