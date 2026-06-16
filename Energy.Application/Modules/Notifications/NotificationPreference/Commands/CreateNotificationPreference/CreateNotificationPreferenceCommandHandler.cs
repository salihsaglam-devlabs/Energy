using Energy.Application.Modules.Notifications.NotificationPreference.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.NotificationPreference.Commands.CreateNotificationPreference;

/// <summary>
/// <see cref="CreateNotificationPreferenceCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="INotificationPreferenceService"/>'i orkestre eder.
/// </summary>
public sealed class CreateNotificationPreferenceCommandHandler
    : IRequestHandler<CreateNotificationPreferenceCommand, BaseResponse<Guid>>
{
    private readonly INotificationPreferenceService _service;

    public CreateNotificationPreferenceCommandHandler(INotificationPreferenceService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateNotificationPreferenceCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
