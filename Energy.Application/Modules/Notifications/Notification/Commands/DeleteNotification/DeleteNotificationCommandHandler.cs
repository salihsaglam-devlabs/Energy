using Energy.Application.Modules.Notifications.Notification.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.Notification.Commands.DeleteNotification;

/// <summary>
/// <see cref="DeleteNotificationCommand"/> handler'ı. <see cref="INotificationService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteNotificationCommandHandler
    : IRequestHandler<DeleteNotificationCommand, BaseResponse<bool>>
{
    private readonly INotificationService _service;

    public DeleteNotificationCommandHandler(INotificationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteNotificationCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
