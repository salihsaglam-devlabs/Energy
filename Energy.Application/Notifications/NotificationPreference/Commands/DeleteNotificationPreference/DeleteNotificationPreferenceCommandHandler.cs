using Energy.Application.Notifications.NotificationPreference.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Notifications.NotificationPreference.Commands.DeleteNotificationPreference;

/// <summary>
/// <see cref="DeleteNotificationPreferenceCommand"/> handler'ı. <see cref="INotificationPreferenceService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteNotificationPreferenceCommandHandler
    : IRequestHandler<DeleteNotificationPreferenceCommand, BaseResponse<bool>>
{
    private readonly INotificationPreferenceService _service;

    public DeleteNotificationPreferenceCommandHandler(INotificationPreferenceService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteNotificationPreferenceCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
