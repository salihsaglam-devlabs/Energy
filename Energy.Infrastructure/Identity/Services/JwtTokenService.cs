using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Energy.Application.Identity.Services;
using Energy.Infrastructure.Identity;
using Energy.Localization;
using Energy.Shared.Models.V1.Identity.Responses;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Energy.Infrastructure.Identity.Services;

public sealed class JwtTokenService : IJwtTokenService
{
    public const string PermissionClaimType = "permission";
    public const string RoleKeyClaimType = "role_key";

    private readonly JwtSettings _settings;

    public JwtTokenService(IOptions<JwtSettings> options)
    {
        _settings = options.Value;

        if (string.IsNullOrWhiteSpace(_settings.Key) || _settings.Key.Length < 32)
        {
            throw new InvalidOperationException(
                LocalizationText.Get(
                    LocalizationKeys.Messages.JwtKeyConfigInvalid,
                    "Jwt:Key must be configured and be at least 32 characters long."));
        }
    }

    public AuthTokenResponse GenerateToken(
        Guid userId,
        string? userName,
        string? email,
        IEnumerable<string> roles,
        IEnumerable<string> roleKeys,
        IEnumerable<string> permissions)
    {
        var rolesList = roles.Where(r => !string.IsNullOrWhiteSpace(r)).Distinct().ToList();
        var roleKeysList = roleKeys.Where(r => !string.IsNullOrWhiteSpace(r)).Distinct().ToList();
        var permissionsList = permissions.Where(p => !string.IsNullOrWhiteSpace(p)).Distinct().ToList();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
        };

        if (!string.IsNullOrWhiteSpace(userName))
        {
            claims.Add(new Claim(ClaimTypes.Name, userName));
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, userName));
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            claims.Add(new Claim(ClaimTypes.Email, email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
        }

        foreach (var role in rolesList)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        foreach (var roleKey in roleKeysList)
        {
            claims.Add(new Claim(RoleKeyClaimType, roleKey));
        }

        foreach (var permission in permissionsList)
        {
            claims.Add(new Claim(PermissionClaimType, permission));
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthTokenResponse
        {
            AccessToken = accessToken,
            TokenType = "Bearer",
            ExpiresAt = expiresAt,
            UserId = userId,
            UserName = userName,
            Email = email,
            Roles = rolesList,
            RoleKeys = roleKeysList,
            Permissions = permissionsList
        };
    }
}

