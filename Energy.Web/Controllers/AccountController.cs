using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Web.Clients.Identity;
using Energy.Web.Models.Account;
using Energy.Web.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers;

[AllowAnonymous]
public sealed class AccountController : Controller
{
    private readonly IAuthApiClient _auth;
    private readonly IAuthCookieFactory _cookies;
    private readonly IWebHostEnvironment _env;

    public AccountController(IAuthApiClient auth, IAuthCookieFactory cookies, IWebHostEnvironment env)
    {
        _auth = auth;
        _cookies = cookies;
        _env = env;
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

        var response = await _auth.LoginAsync(new LoginRequest
        {
            UserNameOrEmail = input.UserNameOrEmail,
            Password = input.Password
        }, ct);

        if (!response.IsSuccess || response.Data is null)
        {
            ModelState.AddModelError(string.Empty, response.Message);
            return View(BuildLoginModel(input.ReturnUrl));
        }

        await _cookies.SignInAsync(HttpContext, response.Data);
        return Redirect(string.IsNullOrEmpty(input.ReturnUrl) ? "/" : input.ReturnUrl);
    }

    [HttpGet("/account/logout"), HttpPost("/account/logout")]
    public async Task<IActionResult> Logout()
    {
        await _cookies.SignOutAsync(HttpContext);
        return Redirect("/account/login");
    }

    [HttpGet("/account/access-denied")]
    public IActionResult AccessDenied() => View(new AccessDeniedViewModel());
}
