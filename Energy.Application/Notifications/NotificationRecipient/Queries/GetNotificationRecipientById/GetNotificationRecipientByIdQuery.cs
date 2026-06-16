using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Responses;
using MediatR;

namespace Energy.Application.Notifications.NotificationRecipient.Queries.GetNotificationRecipientById;

/// <summary>Kimliğe göre NotificationRecipient detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetNotificationRecipientByIdQuery(Guid Id)
    : IRequest<BaseResponse<NotificationRecipientDetailResponse>>;
