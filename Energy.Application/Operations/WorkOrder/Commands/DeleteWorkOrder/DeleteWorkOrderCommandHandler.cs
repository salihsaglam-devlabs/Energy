using Energy.Application.Operations.WorkOrder.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrder.Commands.DeleteWorkOrder;

/// <summary>
/// <see cref="DeleteWorkOrderCommand"/> handler'ı. <see cref="IWorkOrderService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteWorkOrderCommandHandler
    : IRequestHandler<DeleteWorkOrderCommand, BaseResponse<bool>>
{
    private readonly IWorkOrderService _service;

    public DeleteWorkOrderCommandHandler(IWorkOrderService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteWorkOrderCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
