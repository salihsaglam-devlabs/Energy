using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.Notification.Requests;
using MediatR;

namespace Energy.Application.Modules.Notifications.Notification.Commands.CreateNotification;

/// <summary>Yeni Notification oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateNotificationCommand(CreateNotificationRequest Request)
    : IRequest<BaseResponse<Guid>>;
