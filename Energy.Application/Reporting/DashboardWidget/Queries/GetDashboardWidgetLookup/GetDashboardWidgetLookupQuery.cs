using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Responses;
using MediatR;

namespace Energy.Application.Reporting.DashboardWidget.Queries.GetDashboardWidgetLookup;

/// <summary>DashboardWidget lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetDashboardWidgetLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<DashboardWidgetLookupResponse>>>;
