using Energy.Application.Modules.Notifications.Notification.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.Notification.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.Notification.Queries.GetNotificationLookup;

/// <summary>
/// <see cref="GetNotificationLookupQuery"/> handler'ı. <see cref="INotificationLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetNotificationLookupQueryHandler
    : IRequestHandler<GetNotificationLookupQuery, BaseResponse<IReadOnlyList<NotificationLookupResponse>>>
{
    private readonly INotificationLookupService _lookup;

    public GetNotificationLookupQueryHandler(INotificationLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<NotificationLookupResponse>>> Handle(
        GetNotificationLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
