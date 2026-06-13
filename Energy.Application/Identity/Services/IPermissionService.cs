using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Application.Identity.Services;

/// <summary>Yetki kataloğunu okuma ve veritabanıyla eşitleme (sync) işlemlerini sağlayan servis.</summary>
public interface IPermissionService
{
    /// <summary>Roller, menüler ve endpoint'lerdeki kullanım sayılarıyla birlikte tüm yetkileri döndürür.</summary>
    Task<IReadOnlyList<PermissionResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Belirtilen koda sahip yetkiyi döndürür; yoksa null.</summary>
    Task<PermissionResponse?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>Kataloğu <c>Energy.Shared.Identity.Permissions.PermissionCatalog</c> ile eşitler (eksikleri ekler).</summary>
    Task<int> SyncCatalogAsync(CancellationToken cancellationToken = default);
}
