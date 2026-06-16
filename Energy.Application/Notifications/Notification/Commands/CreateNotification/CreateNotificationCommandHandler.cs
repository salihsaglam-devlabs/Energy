using Energy.Application.Notifications.Notification.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Notifications.Notification.Commands.CreateNotification;

/// <summary>
/// <see cref="CreateNotificationCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="INotificationService"/>'i orkestre eder.
/// </summary>
public sealed class CreateNotificationCommandHandler
    : IRequestHandler<CreateNotificationCommand, BaseResponse<Guid>>
{
    private readonly INotificationService _service;

    public CreateNotificationCommandHandler(INotificationService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateNotificationCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
