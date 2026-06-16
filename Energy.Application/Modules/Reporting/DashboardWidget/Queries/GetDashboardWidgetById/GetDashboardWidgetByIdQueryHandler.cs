using Energy.Application.Modules.Reporting.DashboardWidget.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Responses;
using MediatR;

namespace Energy.Application.Modules.Reporting.DashboardWidget.Queries.GetDashboardWidgetById;

/// <summary>
/// <see cref="GetDashboardWidgetByIdQuery"/> handler'ı. <see cref="IDashboardWidgetService"/>'i orkestre eder.
/// </summary>
public sealed class GetDashboardWidgetByIdQueryHandler
    : IRequestHandler<GetDashboardWidgetByIdQuery, BaseResponse<DashboardWidgetDetailResponse>>
{
    private readonly IDashboardWidgetService _service;

    public GetDashboardWidgetByIdQueryHandler(IDashboardWidgetService service)
        => _service = service;

    public Task<BaseResponse<DashboardWidgetDetailResponse>> Handle(
        GetDashboardWidgetByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
