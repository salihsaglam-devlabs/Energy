using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Responses;
using MediatR;

namespace Energy.Application.Reporting.DashboardWidget.Queries.GetDashboardWidgetById;

/// <summary>Kimliğe göre DashboardWidget detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetDashboardWidgetByIdQuery(Guid Id)
    : IRequest<BaseResponse<DashboardWidgetDetailResponse>>;
