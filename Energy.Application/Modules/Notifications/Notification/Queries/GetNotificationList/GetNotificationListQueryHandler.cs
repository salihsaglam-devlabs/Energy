using Energy.Application.Modules.Notifications.Notification.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.Notification.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.Notification.Queries.GetNotificationList;

/// <summary>
/// <see cref="GetNotificationListQuery"/> handler'ı. <see cref="INotificationService"/>'i orkestre eder.
/// </summary>
public sealed class GetNotificationListQueryHandler
    : IRequestHandler<GetNotificationListQuery, BaseResponse<PaginatedResponse<NotificationListResponse>>>
{
    private readonly INotificationService _service;

    public GetNotificationListQueryHandler(INotificationService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<NotificationListResponse>>> Handle(
        GetNotificationListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
