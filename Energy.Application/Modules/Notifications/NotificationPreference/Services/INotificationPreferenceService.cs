using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Requests;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Responses;

namespace Energy.Application.Modules.Notifications.NotificationPreference.Services;

/// <summary>NotificationPreference CRUD use-case sözleşmesi.</summary>
public interface INotificationPreferenceService
{
    /// <summary>Sayfalanmış NotificationPreference listesi.</summary>
    Task<BaseResponse<PaginatedResponse<NotificationPreferenceListResponse>>> GetListAsync(GetNotificationPreferenceListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<NotificationPreferenceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateNotificationPreferenceRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateNotificationPreferenceRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
