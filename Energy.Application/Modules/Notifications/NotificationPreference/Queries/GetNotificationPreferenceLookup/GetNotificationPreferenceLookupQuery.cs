using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Responses;
using MediatR;

namespace Energy.Application.Modules.Notifications.NotificationPreference.Queries.GetNotificationPreferenceLookup;

/// <summary>NotificationPreference lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetNotificationPreferenceLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<NotificationPreferenceLookupResponse>>>;
