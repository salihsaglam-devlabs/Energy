using Energy.Application.Modules.Notifications.NotificationRecipient.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.NotificationRecipient.Commands.UpdateNotificationRecipient;

/// <summary>
/// <see cref="UpdateNotificationRecipientCommand"/> handler'ı. <see cref="INotificationRecipientService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateNotificationRecipientCommandHandler
    : IRequestHandler<UpdateNotificationRecipientCommand, BaseResponse<bool>>
{
    private readonly INotificationRecipientService _service;

    public UpdateNotificationRecipientCommandHandler(INotificationRecipientService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateNotificationRecipientCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
