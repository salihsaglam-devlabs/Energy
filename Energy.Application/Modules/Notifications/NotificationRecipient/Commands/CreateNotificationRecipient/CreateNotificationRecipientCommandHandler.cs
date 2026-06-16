using Energy.Application.Modules.Notifications.NotificationRecipient.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.NotificationRecipient.Commands.CreateNotificationRecipient;

/// <summary>
/// <see cref="CreateNotificationRecipientCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="INotificationRecipientService"/>'i orkestre eder.
/// </summary>
public sealed class CreateNotificationRecipientCommandHandler
    : IRequestHandler<CreateNotificationRecipientCommand, BaseResponse<Guid>>
{
    private readonly INotificationRecipientService _service;

    public CreateNotificationRecipientCommandHandler(INotificationRecipientService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateNotificationRecipientCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
