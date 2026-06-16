using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Energy.Application.Identity.Services;
using Energy.Domain.IAM;
using Energy.Shared.Models.V1.Identity.Responses;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Energy.Infrastructure.Identity.Services;

/// <summary>
/// Mutlak asgari yükle erişim jetonları üretir: <c>sub</c>, <c>name</c>,
/// <c>sst</c> (security stamp). Yetkiler jetona ASLA gömülmez; her istekte sunucu
/// tarafında çözülür ve önbelleğe alınır.
/// </summary>
public sealed class JwtTokenService : IJwtTokenService
{
    /// <summary>Güvenlik damgasını (security stamp) taşıyan talep (claim) adı.</summary>
    public const string SecurityStampClaim = "sst";

    private readonly JwtSettings _settings;

    /// <summary>JWT ayarlarıyla servisi başlatır.</summary>
    public JwtTokenService(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    /// <summary>Verilen kullanıcı için imzalı bir erişim jetonu üretir.</summary>
    public AuthTokenResponse Issue(User user)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim(SecurityStampClaim, user.SecurityStamp.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AuthTokenResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
            ExpiresAt = expiresAt,
            UserId = user.Id,
            UserName = user.UserName,
            DisplayName = $"{user.FirstName} {user.LastName}".Trim()
        };
    }
}
