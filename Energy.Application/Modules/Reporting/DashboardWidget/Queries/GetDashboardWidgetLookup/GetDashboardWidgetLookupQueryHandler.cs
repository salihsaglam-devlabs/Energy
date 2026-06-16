using Energy.Application.Modules.Reporting.DashboardWidget.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Responses;
using MediatR;

namespace Energy.Application.Modules.Reporting.DashboardWidget.Queries.GetDashboardWidgetLookup;

/// <summary>
/// <see cref="GetDashboardWidgetLookupQuery"/> handler'ı. <see cref="IDashboardWidgetLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetDashboardWidgetLookupQueryHandler
    : IRequestHandler<GetDashboardWidgetLookupQuery, BaseResponse<IReadOnlyList<DashboardWidgetLookupResponse>>>
{
    private readonly IDashboardWidgetLookupService _lookup;

    public GetDashboardWidgetLookupQueryHandler(IDashboardWidgetLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<DashboardWidgetLookupResponse>>> Handle(
        GetDashboardWidgetLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
