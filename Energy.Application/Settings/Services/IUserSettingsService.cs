using Energy.Shared.Models.V1.Settings.Requests;
using Energy.Shared.Models.V1.Settings.Responses;

namespace Energy.Application.Settings.Services;

/// <summary>
/// Kullanıcı bazlı self-service tercihler. Her kullanıcı yalnızca kendi satırını
/// okur ve günceller; satır henüz yoksa varsayılan değerler döndürülür (ve
/// güncellemede kalıcılaştırılır).
/// </summary>
public interface IUserSettingsService
{
    /// <summary>Kullanıcının ayarlarını döndürür; satır yoksa varsayılanları üretir.</summary>
    Task<UserSettingsResponse> GetAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>Kullanıcının ayarlarını oluşturur veya günceller ve saklanan değerleri döndürür.</summary>
    Task<UserSettingsResponse> UpdateAsync(Guid userId, UpdateUserSettingsRequest request, CancellationToken cancellationToken = default);
}
