using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Requests;
using MediatR;

namespace Energy.Application.Reporting.DashboardWidget.Commands.UpdateDashboardWidget;

/// <summary>Var olan DashboardWidget kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateDashboardWidgetCommand(Guid Id, UpdateDashboardWidgetRequest Request)
    : IRequest<BaseResponse<bool>>;
