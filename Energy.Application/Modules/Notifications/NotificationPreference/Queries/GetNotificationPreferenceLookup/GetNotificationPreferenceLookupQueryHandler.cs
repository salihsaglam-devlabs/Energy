using Energy.Application.Modules.Notifications.NotificationPreference.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.NotificationPreference.Queries.GetNotificationPreferenceLookup;

/// <summary>
/// <see cref="GetNotificationPreferenceLookupQuery"/> handler'ı. <see cref="INotificationPreferenceLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetNotificationPreferenceLookupQueryHandler
    : IRequestHandler<GetNotificationPreferenceLookupQuery, BaseResponse<IReadOnlyList<NotificationPreferenceLookupResponse>>>
{
    private readonly INotificationPreferenceLookupService _lookup;

    public GetNotificationPreferenceLookupQueryHandler(INotificationPreferenceLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<NotificationPreferenceLookupResponse>>> Handle(
        GetNotificationPreferenceLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
