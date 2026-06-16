using Energy.Application.Notifications.NotificationPreference.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Notifications.NotificationPreference.Commands.UpdateNotificationPreference;

/// <summary>
/// <see cref="UpdateNotificationPreferenceCommand"/> handler'ı. <see cref="INotificationPreferenceService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateNotificationPreferenceCommandHandler
    : IRequestHandler<UpdateNotificationPreferenceCommand, BaseResponse<bool>>
{
    private readonly INotificationPreferenceService _service;

    public UpdateNotificationPreferenceCommandHandler(INotificationPreferenceService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateNotificationPreferenceCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
