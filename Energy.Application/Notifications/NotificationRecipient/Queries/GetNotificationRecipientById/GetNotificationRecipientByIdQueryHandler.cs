using Energy.Application.Notifications.NotificationRecipient.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Responses;
using MediatR;

namespace Energy.Application.Notifications.NotificationRecipient.Queries.GetNotificationRecipientById;

/// <summary>
/// <see cref="GetNotificationRecipientByIdQuery"/> handler'ı. <see cref="INotificationRecipientService"/>'i orkestre eder.
/// </summary>
public sealed class GetNotificationRecipientByIdQueryHandler
    : IRequestHandler<GetNotificationRecipientByIdQuery, BaseResponse<NotificationRecipientDetailResponse>>
{
    private readonly INotificationRecipientService _service;

    public GetNotificationRecipientByIdQueryHandler(INotificationRecipientService service)
        => _service = service;

    public Task<BaseResponse<NotificationRecipientDetailResponse>> Handle(
        GetNotificationRecipientByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
