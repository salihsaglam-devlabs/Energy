using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.NotificationPreference.Queries.GetNotificationPreferenceById;

/// <summary>Kimliğe göre NotificationPreference detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetNotificationPreferenceByIdQuery(Guid Id)
    : IRequest<BaseResponse<NotificationPreferenceDetailResponse>>;
