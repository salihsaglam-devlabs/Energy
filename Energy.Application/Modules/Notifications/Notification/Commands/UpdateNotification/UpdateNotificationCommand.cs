using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.Notification.Requests;
using MediatR;

namespace Energy.Application.Modules.Notifications.Notification.Commands.UpdateNotification;

/// <summary>Var olan Notification kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateNotificationCommand(Guid Id, UpdateNotificationRequest Request)
    : IRequest<BaseResponse<bool>>;
