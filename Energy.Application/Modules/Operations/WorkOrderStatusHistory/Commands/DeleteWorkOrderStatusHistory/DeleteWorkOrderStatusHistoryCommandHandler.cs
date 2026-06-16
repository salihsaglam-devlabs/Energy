using Energy.Application.Modules.Operations.WorkOrderStatusHistory.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderStatusHistory.Commands.DeleteWorkOrderStatusHistory;

/// <summary>
/// <see cref="DeleteWorkOrderStatusHistoryCommand"/> handler'ı. <see cref="IWorkOrderStatusHistoryService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteWorkOrderStatusHistoryCommandHandler
    : IRequestHandler<DeleteWorkOrderStatusHistoryCommand, BaseResponse<bool>>
{
    private readonly IWorkOrderStatusHistoryService _service;

    public DeleteWorkOrderStatusHistoryCommandHandler(IWorkOrderStatusHistoryService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteWorkOrderStatusHistoryCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
