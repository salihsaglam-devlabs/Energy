using Energy.Application.Modules.Notifications.NotificationPreference.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.NotificationPreference.Queries.GetNotificationPreferenceById;

/// <summary>
/// <see cref="GetNotificationPreferenceByIdQuery"/> handler'ı. <see cref="INotificationPreferenceService"/>'i orkestre eder.
/// </summary>
public sealed class GetNotificationPreferenceByIdQueryHandler
    : IRequestHandler<GetNotificationPreferenceByIdQuery, BaseResponse<NotificationPreferenceDetailResponse>>
{
    private readonly INotificationPreferenceService _service;

    public GetNotificationPreferenceByIdQueryHandler(INotificationPreferenceService service)
        => _service = service;

    public Task<BaseResponse<NotificationPreferenceDetailResponse>> Handle(
        GetNotificationPreferenceByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
