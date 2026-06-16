using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Notifications.Notification.Commands.DeleteNotification;

/// <summary>Notification kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteNotificationCommand(Guid Id) : IRequest<BaseResponse<bool>>;
