using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.Notification.Requests;
using Energy.Shared.Models.V1.Notifications.Notification.Responses;

namespace Energy.Application.Modules.Notifications.Notification.Services;

/// <summary>Notification CRUD use-case sözleşmesi.</summary>
public interface INotificationService
{
    /// <summary>Sayfalanmış Notification listesi.</summary>
    Task<BaseResponse<PaginatedResponse<NotificationListResponse>>> GetListAsync(GetNotificationListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<NotificationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateNotificationRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateNotificationRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
