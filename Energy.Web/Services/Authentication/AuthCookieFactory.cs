using System.Security.Claims;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Web.Clients.Infrastructure.Authentication;
using Energy.Web.Common;

namespace Energy.Web.Services.Authentication;

/// <summary>
/// Builds the cookie principal used by the Web layer from the API's
/// <see cref="AuthTokenResponse"/>. Role identifiers are looked up once at
/// sign-in via <see cref="IRoleIdResolver"/> so the navigation service can
/// query role-scoped endpoints without an extra round-trip on every request.
/// </summary>
public interface IAuthCookieFactory
{
    Task<ClaimsPrincipal> CreatePrincipalAsync(
        AuthTokenResponse token,
        CancellationToken cancellationToken = default);
}

public sealed class AuthCookieFactory : IAuthCookieFactory
{
    private readonly IRoleIdResolver _roleIdResolver;
    private readonly IUserApiTokenProvider _tokenProvider;

    public AuthCookieFactory(
        IRoleIdResolver roleIdResolver,
        IUserApiTokenProvider tokenProvider)
    {
        _roleIdResolver = roleIdResolver;
        _tokenProvider = tokenProvider;
    }

    public async Task<ClaimsPrincipal> CreatePrincipalAsync(
        AuthTokenResponse token,
        CancellationToken cancellationToken = default)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, token.UserId.ToString())
        };

        if (!string.IsNullOrEmpty(token.UserName))
        {
            claims.Add(new Claim(ClaimTypes.Name, token.UserName));
            claims.Add(new Claim(EnergyClaimTypes.FullName, token.UserName));
        }

        if (!string.IsNullOrEmpty(token.Email))
        {
            claims.Add(new Claim(ClaimTypes.Email, token.Email));
        }

        foreach (var role in token.Roles)
        {
            if (!string.IsNullOrEmpty(role))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        // Stable role identifiers (NormalizedName) — used for culture-independent
        // role detection such as "is this user an Admin?".
        foreach (var roleKey in token.RoleKeys)
        {
            if (!string.IsNullOrEmpty(roleKey))
            {
                claims.Add(new Claim(EnergyClaimTypes.RoleKey, roleKey));
            }
        }

        foreach (var permission in token.Permissions)
        {
            if (!string.IsNullOrEmpty(permission))
            {
                claims.Add(new Claim(EnergyClaimTypes.Permission, permission));
            }
        }

        var roleIds = await ResolveRoleIdsAsync(token, cancellationToken);
        foreach (var roleId in roleIds)
        {
            claims.Add(new Claim(EnergyClaimTypes.RoleId, roleId.ToString()));
        }

        var identity = new ClaimsIdentity(
            claims,
            authenticationType: "energy.cookie",
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role);

        return new ClaimsPrincipal(identity);
    }

    private async Task<IReadOnlyList<Guid>> ResolveRoleIdsAsync(
        AuthTokenResponse token,
        CancellationToken cancellationToken)
    {
        // The auth cookie hasn't been written yet, so the outbound HTTP
        // handler can't read the bearer token from the cookie ticket.
        // Temporarily inject the freshly issued access token so the role
        // lookup call is authenticated.
        using (_tokenProvider.UseAccessToken(token.AccessToken))
        {
            return await _roleIdResolver.ResolveAsync(token.Roles, cancellationToken);
        }
    }
}

