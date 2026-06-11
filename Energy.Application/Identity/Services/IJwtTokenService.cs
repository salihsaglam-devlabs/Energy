using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Application.Identity.Services;

public interface IJwtTokenService
{
    AuthTokenResponse GenerateToken(
        Guid userId,
        string? userName,
        string? email,
        IEnumerable<string> roles,
        IEnumerable<string> roleKeys,
        IEnumerable<string> permissions);
}

