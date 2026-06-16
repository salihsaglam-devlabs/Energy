using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Requests;
using MediatR;

namespace Energy.Application.Notifications.NotificationPreference.Commands.UpdateNotificationPreference;

/// <summary>Var olan NotificationPreference kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateNotificationPreferenceCommand(Guid Id, UpdateNotificationPreferenceRequest Request)
    : IRequest<BaseResponse<bool>>;
