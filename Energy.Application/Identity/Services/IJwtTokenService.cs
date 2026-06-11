using Energy.Domain.Identity;
using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Application.Identity.Services;

public interface IJwtTokenService
{
    AuthTokenResponse Issue(User user);
}
