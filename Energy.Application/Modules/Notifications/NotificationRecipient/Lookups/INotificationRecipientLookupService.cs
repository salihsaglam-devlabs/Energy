using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Responses;

namespace Energy.Application.Modules.Notifications.NotificationRecipient.Lookups;

/// <summary>NotificationRecipient lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface INotificationRecipientLookupService
{
    /// <summary>NotificationRecipient lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<NotificationRecipientLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
