using Energy.Application.Reporting.DashboardWidget.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Reporting.DashboardWidget.Commands.UpdateDashboardWidget;

/// <summary>
/// <see cref="UpdateDashboardWidgetCommand"/> handler'ı. <see cref="IDashboardWidgetService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateDashboardWidgetCommandHandler
    : IRequestHandler<UpdateDashboardWidgetCommand, BaseResponse<bool>>
{
    private readonly IDashboardWidgetService _service;

    public UpdateDashboardWidgetCommandHandler(IDashboardWidgetService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateDashboardWidgetCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
