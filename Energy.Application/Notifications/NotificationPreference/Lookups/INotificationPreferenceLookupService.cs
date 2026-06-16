using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Responses;

namespace Energy.Application.Notifications.NotificationPreference.Lookups;

/// <summary>NotificationPreference lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface INotificationPreferenceLookupService
{
    /// <summary>NotificationPreference lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<NotificationPreferenceLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
