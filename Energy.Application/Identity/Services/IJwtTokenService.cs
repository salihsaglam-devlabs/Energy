using Energy.Domain.IAM;
using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Application.Identity.Services;

/// <summary>Kullanıcılar için JWT erişim token'ı üreten servis.</summary>
public interface IJwtTokenService
{
    /// <summary>Verilen kullanıcı için imzalı bir JWT token ve meta verisini üretir.</summary>
    AuthTokenResponse Issue(User user);
}
