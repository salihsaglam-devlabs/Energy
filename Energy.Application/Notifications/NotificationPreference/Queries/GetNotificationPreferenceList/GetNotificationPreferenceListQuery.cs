using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Requests;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Responses;
using MediatR;

namespace Energy.Application.Notifications.NotificationPreference.Queries.GetNotificationPreferenceList;

/// <summary>Sayfalanmış NotificationPreference listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetNotificationPreferenceListQuery(GetNotificationPreferenceListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<NotificationPreferenceListResponse>>>;
