using Energy.Application.Operations.WorkOrderStatusHistory.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderStatusHistory.Commands.UpdateWorkOrderStatusHistory;

/// <summary>
/// <see cref="UpdateWorkOrderStatusHistoryCommand"/> handler'ı. <see cref="IWorkOrderStatusHistoryService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateWorkOrderStatusHistoryCommandHandler
    : IRequestHandler<UpdateWorkOrderStatusHistoryCommand, BaseResponse<bool>>
{
    private readonly IWorkOrderStatusHistoryService _service;

    public UpdateWorkOrderStatusHistoryCommandHandler(IWorkOrderStatusHistoryService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateWorkOrderStatusHistoryCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
