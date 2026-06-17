using Energy.Application.Notifications.NotificationRecipient.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Responses;
using MediatR;

namespace Energy.Application.Notifications.NotificationRecipient.Queries.GetNotificationRecipientLookup;

/// <summary>
/// <see cref="GetNotificationRecipientLookupQuery"/> handler'ı. <see cref="INotificationRecipientLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetNotificationRecipientLookupQueryHandler
    : IRequestHandler<GetNotificationRecipientLookupQuery, BaseResponse<IReadOnlyList<NotificationRecipientLookupResponse>>>
{
    private readonly INotificationRecipientLookupService _lookup;

    public GetNotificationRecipientLookupQueryHandler(INotificationRecipientLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<NotificationRecipientLookupResponse>>> Handle(
        GetNotificationRecipientLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
