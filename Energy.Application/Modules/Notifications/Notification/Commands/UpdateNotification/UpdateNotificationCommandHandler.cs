using Energy.Application.Modules.Notifications.Notification.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.Notification.Commands.UpdateNotification;

/// <summary>
/// <see cref="UpdateNotificationCommand"/> handler'ı. <see cref="INotificationService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateNotificationCommandHandler
    : IRequestHandler<UpdateNotificationCommand, BaseResponse<bool>>
{
    private readonly INotificationService _service;

    public UpdateNotificationCommandHandler(INotificationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateNotificationCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
