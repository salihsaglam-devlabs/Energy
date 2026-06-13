namespace Energy.Application.Identity.Services;

/// <summary>
/// Bir kullanıcının etkin (effective) yetki kümesini Kullanıcı → Rol → Yetki
/// zinciri üzerinden çözümler. Uygulamalar <c>userId</c> bazında önbellekleme
/// (cache) ve geçersiz kılma (invalidation) yapmalıdır.
/// </summary>
public interface IPermissionResolver
{
    /// <summary>Kullanıcının sahip olduğu tüm etkin yetki kodlarını döndürür.</summary>
    Task<IReadOnlySet<string>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>Kullanıcının belirtilen yetki koduna sahip olup olmadığını döndürür.</summary>
    Task<bool> HasPermissionAsync(Guid userId, string permissionCode, CancellationToken cancellationToken = default);

    /// <summary>Belirtilen kullanıcı için önbelleğe alınmış yetki kümesini düşürür (temizler).</summary>
    void InvalidateUser(Guid userId);

    /// <summary>Belirtilen role sahip her kullanıcının önbelleğe alınmış kümelerini düşürür.</summary>
    Task InvalidateRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
}
