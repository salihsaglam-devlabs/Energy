using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.Notification.Requests;
using Energy.Shared.Models.V1.Notifications.Notification.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.Notification.Queries.GetNotificationList;

/// <summary>Sayfalanmış Notification listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetNotificationListQuery(GetNotificationListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<NotificationListResponse>>>;
