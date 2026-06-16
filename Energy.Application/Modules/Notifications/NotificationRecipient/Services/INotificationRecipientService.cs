using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Requests;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Responses;

namespace Energy.Application.Modules.Notifications.NotificationRecipient.Services;

/// <summary>NotificationRecipient CRUD use-case sözleşmesi.</summary>
public interface INotificationRecipientService
{
    /// <summary>Sayfalanmış NotificationRecipient listesi.</summary>
    Task<BaseResponse<PaginatedResponse<NotificationRecipientListResponse>>> GetListAsync(GetNotificationRecipientListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<NotificationRecipientDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateNotificationRecipientRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateNotificationRecipientRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
