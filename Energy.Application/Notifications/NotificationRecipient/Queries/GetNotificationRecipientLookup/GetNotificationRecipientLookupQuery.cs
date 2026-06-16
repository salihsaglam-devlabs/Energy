using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Responses;
using MediatR;

namespace Energy.Application.Notifications.NotificationRecipient.Queries.GetNotificationRecipientLookup;

/// <summary>NotificationRecipient lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetNotificationRecipientLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<NotificationRecipientLookupResponse>>>;
