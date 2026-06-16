using Energy.Application.Modules.Notifications.NotificationRecipient.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.NotificationRecipient.Queries.GetNotificationRecipientList;

/// <summary>
/// <see cref="GetNotificationRecipientListQuery"/> handler'ı. <see cref="INotificationRecipientService"/>'i orkestre eder.
/// </summary>
public sealed class GetNotificationRecipientListQueryHandler
    : IRequestHandler<GetNotificationRecipientListQuery, BaseResponse<PaginatedResponse<NotificationRecipientListResponse>>>
{
    private readonly INotificationRecipientService _service;

    public GetNotificationRecipientListQueryHandler(INotificationRecipientService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<NotificationRecipientListResponse>>> Handle(
        GetNotificationRecipientListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
