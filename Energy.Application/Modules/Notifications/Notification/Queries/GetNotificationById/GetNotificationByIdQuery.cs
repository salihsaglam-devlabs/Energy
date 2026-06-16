using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.Notification.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.Notification.Queries.GetNotificationById;

/// <summary>Kimliğe göre Notification detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetNotificationByIdQuery(Guid Id)
    : IRequest<BaseResponse<NotificationDetailResponse>>;
