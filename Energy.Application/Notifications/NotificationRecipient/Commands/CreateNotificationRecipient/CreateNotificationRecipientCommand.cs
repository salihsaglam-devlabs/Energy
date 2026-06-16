using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Requests;
using MediatR;

namespace Energy.Application.Notifications.NotificationRecipient.Commands.CreateNotificationRecipient;

/// <summary>Yeni NotificationRecipient oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateNotificationRecipientCommand(CreateNotificationRecipientRequest Request)
    : IRequest<BaseResponse<Guid>>;
