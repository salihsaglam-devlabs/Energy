using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Requests;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Responses;
using MediatR;

namespace Energy.Application.Modules.Reporting.DashboardWidget.Queries.GetDashboardWidgetList;

/// <summary>Sayfalanmış DashboardWidget listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetDashboardWidgetListQuery(GetDashboardWidgetListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<DashboardWidgetListResponse>>>;
