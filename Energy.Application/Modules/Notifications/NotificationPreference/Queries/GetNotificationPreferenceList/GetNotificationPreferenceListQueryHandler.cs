using Energy.Application.Modules.Notifications.NotificationPreference.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.NotificationPreference.Queries.GetNotificationPreferenceList;

/// <summary>
/// <see cref="GetNotificationPreferenceListQuery"/> handler'ı. <see cref="INotificationPreferenceService"/>'i orkestre eder.
/// </summary>
public sealed class GetNotificationPreferenceListQueryHandler
    : IRequestHandler<GetNotificationPreferenceListQuery, BaseResponse<PaginatedResponse<NotificationPreferenceListResponse>>>
{
    private readonly INotificationPreferenceService _service;

    public GetNotificationPreferenceListQueryHandler(INotificationPreferenceService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<NotificationPreferenceListResponse>>> Handle(
        GetNotificationPreferenceListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
