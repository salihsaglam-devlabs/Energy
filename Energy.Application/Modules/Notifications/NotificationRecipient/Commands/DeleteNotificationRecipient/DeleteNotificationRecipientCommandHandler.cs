using Energy.Application.Modules.Notifications.NotificationRecipient.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.NotificationRecipient.Commands.DeleteNotificationRecipient;

/// <summary>
/// <see cref="DeleteNotificationRecipientCommand"/> handler'ı. <see cref="INotificationRecipientService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteNotificationRecipientCommandHandler
    : IRequestHandler<DeleteNotificationRecipientCommand, BaseResponse<bool>>
{
    private readonly INotificationRecipientService _service;

    public DeleteNotificationRecipientCommandHandler(INotificationRecipientService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteNotificationRecipientCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
