using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Requests;
using MediatR;

namespace Energy.Application.Modules.Notifications.NotificationPreference.Commands.CreateNotificationPreference;

/// <summary>Yeni NotificationPreference oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateNotificationPreferenceCommand(CreateNotificationPreferenceRequest Request)
    : IRequest<BaseResponse<Guid>>;
