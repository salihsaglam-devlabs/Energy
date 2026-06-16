using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Reporting.DashboardWidget.Commands.DeleteDashboardWidget;

/// <summary>DashboardWidget kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteDashboardWidgetCommand(Guid Id) : IRequest<BaseResponse<bool>>;
