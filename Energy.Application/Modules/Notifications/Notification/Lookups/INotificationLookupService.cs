using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.Notification.Responses;

namespace Energy.Application.Modules.Notifications.Notification.Lookups;

/// <summary>Notification lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface INotificationLookupService
{
    /// <summary>Notification lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<NotificationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
