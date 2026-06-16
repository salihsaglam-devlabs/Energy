using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Notifications.NotificationRecipient.Commands.DeleteNotificationRecipient;

/// <summary>NotificationRecipient kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteNotificationRecipientCommand(Guid Id) : IRequest<BaseResponse<bool>>;
