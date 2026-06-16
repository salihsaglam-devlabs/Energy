using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Requests;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Responses;
using MediatR;

namespace Energy.Application.Notifications.NotificationRecipient.Queries.GetNotificationRecipientList;

/// <summary>Sayfalanmış NotificationRecipient listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetNotificationRecipientListQuery(GetNotificationRecipientListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<NotificationRecipientListResponse>>>;
