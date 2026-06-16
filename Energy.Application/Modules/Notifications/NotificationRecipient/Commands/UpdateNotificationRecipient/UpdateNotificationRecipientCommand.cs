using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Requests;
using MediatR;

namespace Energy.Application.Modules.Notifications.NotificationRecipient.Commands.UpdateNotificationRecipient;

/// <summary>Var olan NotificationRecipient kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateNotificationRecipientCommand(Guid Id, UpdateNotificationRecipientRequest Request)
    : IRequest<BaseResponse<bool>>;
