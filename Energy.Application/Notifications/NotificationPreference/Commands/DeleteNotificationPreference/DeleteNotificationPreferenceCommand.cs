using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Notifications.NotificationPreference.Commands.DeleteNotificationPreference;

/// <summary>NotificationPreference kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteNotificationPreferenceCommand(Guid Id) : IRequest<BaseResponse<bool>>;
