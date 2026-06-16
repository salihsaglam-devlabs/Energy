using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Requests;
using MediatR;

namespace Energy.Application.Reporting.DashboardWidget.Commands.CreateDashboardWidget;

/// <summary>Yeni DashboardWidget oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateDashboardWidgetCommand(CreateDashboardWidgetRequest Request)
    : IRequest<BaseResponse<Guid>>;
