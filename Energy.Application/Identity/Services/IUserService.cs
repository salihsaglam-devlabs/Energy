using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Application.Identity.Services;

/// <summary>Kullanıcı yönetimi, kimlik doğrulama ve erişim/profil işlemleri servisi.</summary>
public interface IUserService
{
    /// <summary>Kullanıcıları sayfalı olarak (arama/sıralama ile) döndürür.</summary>
    Task<PaginatedResponse<UserSummaryResponse>> GetAllAsync(PaginatedRequest request, CancellationToken cancellationToken = default);

    /// <summary>Belirtilen kimliğe sahip kullanıcının ayrıntılarını döndürür; yoksa null.</summary>
    Task<UserDetailResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Yeni bir kullanıcı oluşturur ve ayrıntılarını döndürür.</summary>
    Task<UserDetailResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);

    /// <summary>Mevcut bir kullanıcıyı günceller ve güncel ayrıntılarını döndürür.</summary>
    Task<UserDetailResponse> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default);

    /// <summary>Bir kullanıcıyı siler; başarılıysa true döner.</summary>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Kullanıcının parolasını değiştirir; başarılıysa true döner.</summary>
    Task<bool> ChangePasswordAsync(Guid id, ChangePasswordRequest request, CancellationToken cancellationToken = default);

    /// <summary>Kullanıcının rollerini, rolden devraldığı yetkileri ve doğrudan atamalarını döndürür.</summary>
    Task<UserAccessResponse?> GetAccessAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Kullanıcının rol atamalarını ve doğrudan yetki atamalarını tek işlemde değiştirir.</summary>
    Task<UserAccessResponse> SetAccessAsync(Guid id, SetUserAccessRequest request, CancellationToken cancellationToken = default);

    /// <summary>Kimlik bilgilerini doğrular ve başarılıysa bir kimlik token'ı döndürür.</summary>
    Task<AuthTokenResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

    /// <summary>Kullanıcının profil resmini döndürür; yoksa null.</summary>
    Task<ProfileImageResponse?> GetProfileImageAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Kullanıcının profil resmini saklar (veya mevcut olanı değiştirir).</summary>
    Task<bool> SetProfileImageAsync(Guid id, byte[] content, string contentType, CancellationToken cancellationToken = default);

    /// <summary>Kullanıcının profil resmini temizler.</summary>
    Task<bool> RemoveProfileImageAsync(Guid id, CancellationToken cancellationToken = default);
}
