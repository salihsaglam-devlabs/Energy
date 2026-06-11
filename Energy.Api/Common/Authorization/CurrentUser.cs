using System.Security.Claims;
using Energy.Application.Identity.Services;

namespace Energy.Api.Common.Authorization;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUser(IHttpContextAccessor accessor) { _accessor = accessor; }

    public Guid? UserId
    {
        get
        {
            var raw = _accessor.HttpContext?.User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value
                   ?? _accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(raw, out var id) ? id : null;
        }
    }

    public string? UserName =>
        _accessor.HttpContext?.User.Identity?.Name
        ?? _accessor.HttpContext?.User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName)?.Value;

    public bool IsAuthenticated => _accessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}
