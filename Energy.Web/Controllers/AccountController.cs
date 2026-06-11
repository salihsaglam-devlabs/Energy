using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Web.Clients.Identity;
using Energy.Web.Clients.Infrastructure.Authentication;
using Energy.Web.Common;
using Energy.Web.Models.Account;
using Energy.Web.Services.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Energy.Web.Controllers;

[AllowAnonymous]
public sealed class AccountController : Controller
{
    private readonly IAuthApiClient _authApiClient;
    private readonly IAuthCookieFactory _cookieFactory;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        IAuthApiClient authApiClient,
        IAuthCookieFactory cookieFactory,
        IStringLocalizer<SharedResource> localizer,
        ILogger<AccountController> logger)
    {
        _authApiClient = authApiClient;
        _cookieFactory = cookieFactory;
        _localizer = localizer;
        _logger = logger;
    }

    [HttpGet("/account/login")]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return Redirect(Url.GetLocalReturnUrl(returnUrl, "/"));
        }

        ViewData["Title"] = _localizer.GetText(LocalizationKeys.Auth.SignInTitle);

        return View(new LoginViewModel
        {
            ReturnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : null
        });
    }

    [HttpPost("/account/login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        [FromForm] LoginInputModel input,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(input.UserNameOrEmail) || string.IsNullOrWhiteSpace(input.Password))
        {
            return Json(new
            {
                ok = false,
                message = _localizer.GetText(LocalizationKeys.Auth.InvalidCredentials)
            });
        }

        BaseResponse<AuthTokenResponse> envelope;
        try
        {
            envelope = await _authApiClient.LoginAsync(
                new LoginRequest
                {
                    UserNameOrEmail = input.UserNameOrEmail,
                    Password = input.Password
                },
                cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Login request to API failed for user '{UserNameOrEmail}'.", input.UserNameOrEmail);
            return Json(new
            {
                ok = false,
                message = _localizer.GetText(LocalizationKeys.Notifications.NetworkError)
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Login response could not be processed for user '{UserNameOrEmail}'.", input.UserNameOrEmail);
            return Json(new
            {
                ok = false,
                message = _localizer.GetText(LocalizationKeys.Notifications.GenericError)
            });
        }

        if (!envelope.IsSuccess || envelope.Data is null || string.IsNullOrEmpty(envelope.Data.AccessToken))
        {
            return Json(new
            {
                ok = false,
                message = _localizer.GetText(LocalizationKeys.Auth.InvalidCredentials)
            });
        }

        var token = envelope.Data;
        var principal = await _cookieFactory.CreatePrincipalAsync(token, cancellationToken);

        var properties = new AuthenticationProperties
        {
            IsPersistent = input.RememberMe,
            ExpiresUtc = token.ExpiresAt.ToUniversalTime(),
            AllowRefresh = false
        };

        properties.StoreTokens(new[]
        {
            new AuthenticationToken { Name = ApiAuthTokens.AccessToken, Value = token.AccessToken },
            new AuthenticationToken { Name = ApiAuthTokens.ExpiresAt, Value = token.ExpiresAt.ToUniversalTime().ToString("o") }
        });

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            properties);

        var redirectUrl = Url.GetLocalReturnUrl(input.ReturnUrl, "/");

        return Json(new
        {
            ok = true,
            redirect = redirectUrl
        });
    }

    [HttpPost("/account/logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/account/login");
    }

    [HttpGet("/account/access-denied")]
    public IActionResult AccessDenied(string? path = null, string? permission = null)
    {
        ViewData["Title"] = _localizer.GetText(LocalizationKeys.Auth.AccessDeniedTitle);

        var sanitizedPath = string.IsNullOrWhiteSpace(path) || !path.StartsWith('/')
            ? "/"
            : path;

        var permissionHint = string.IsNullOrWhiteSpace(permission)
            ? string.Empty
            : permission.Trim();

        return View(new AccessDeniedViewModel
        {
            RequestedPath = sanitizedPath,
            RequestedPermission = permissionHint
        });
    }
}

