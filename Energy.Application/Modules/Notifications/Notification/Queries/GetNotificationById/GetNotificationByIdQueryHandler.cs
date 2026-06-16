using Energy.Application.Modules.Notifications.Notification.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.Notification.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.Notification.Queries.GetNotificationById;

/// <summary>
/// <see cref="GetNotificationByIdQuery"/> handler'ı. <see cref="INotificationService"/>'i orkestre eder.
/// </summary>
public sealed class GetNotificationByIdQueryHandler
    : IRequestHandler<GetNotificationByIdQuery, BaseResponse<NotificationDetailResponse>>
{
    private readonly INotificationService _service;

    public GetNotificationByIdQueryHandler(INotificationService service)
        => _service = service;

    public Task<BaseResponse<NotificationDetailResponse>> Handle(
        GetNotificationByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
