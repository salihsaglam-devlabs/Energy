using Energy.Application.Reporting.DashboardWidget.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Reporting.DashboardWidget.Commands.DeleteDashboardWidget;

/// <summary>
/// <see cref="DeleteDashboardWidgetCommand"/> handler'ı. <see cref="IDashboardWidgetService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteDashboardWidgetCommandHandler
    : IRequestHandler<DeleteDashboardWidgetCommand, BaseResponse<bool>>
{
    private readonly IDashboardWidgetService _service;

    public DeleteDashboardWidgetCommandHandler(IDashboardWidgetService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteDashboardWidgetCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
