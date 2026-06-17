using Energy.Application.Reporting.DashboardWidget.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Responses;
using MediatR;

namespace Energy.Application.Reporting.DashboardWidget.Queries.GetDashboardWidgetList;

/// <summary>
/// <see cref="GetDashboardWidgetListQuery"/> handler'ı. <see cref="IDashboardWidgetService"/>'i orkestre eder.
/// </summary>
public sealed class GetDashboardWidgetListQueryHandler
    : IRequestHandler<GetDashboardWidgetListQuery, BaseResponse<PaginatedResponse<DashboardWidgetListResponse>>>
{
    private readonly IDashboardWidgetService _service;

    public GetDashboardWidgetListQueryHandler(IDashboardWidgetService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<DashboardWidgetListResponse>>> Handle(
        GetDashboardWidgetListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
