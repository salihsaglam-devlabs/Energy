using System.Security.Claims;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Web.Clients.Infrastructure.Authentication;
using Energy.Web.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Energy.Web.Services.Authentication;

public interface IAuthCookieFactory
{
    Task SignInAsync(HttpContext httpContext, AuthTokenResponse token);
    Task SignOutAsync(HttpContext httpContext);
}

public sealed class AuthCookieFactory : IAuthCookieFactory
{
    public async Task SignInAsync(HttpContext httpContext, AuthTokenResponse token)
    {
        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, token.UserId.ToString()));
        identity.AddClaim(new Claim(ClaimTypes.Name, token.UserName));
        identity.AddClaim(new Claim(EnergyClaimTypes.FullName, token.DisplayName));
        identity.AddClaim(new Claim("display_name", token.DisplayName));

        // Etkin yetki kümesini ve rol adlarını çereze yansıt; böylece sayfa düzeyi
        // filtreler, görünüm yardımcıları ve DevExtreme aksiyon kısıtlaması, API'ye
        // gidip gelmeden yetkilendirme kararları verebilir.
        foreach (var role in token.Roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        foreach (var permission in token.Permissions)
        {
            identity.AddClaim(new Claim(EnergyClaimTypes.Permission, permission));
        }

        var principal = new ClaimsPrincipal(identity);
        var properties = new AuthenticationProperties { IsPersistent = true, ExpiresUtc = token.ExpiresAt };
        properties.StoreTokens(new[]
        {
            new AuthenticationToken { Name = ApiAuthTokens.AccessToken, Value = token.AccessToken },
            new AuthenticationToken { Name = ApiAuthTokens.ExpiresAt, Value = token.ExpiresAt.ToString("O") }
        });
        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
    }

    /// <inheritdoc />
    public Task SignOutAsync(HttpContext httpContext)
        => httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
}
