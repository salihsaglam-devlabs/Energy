using Energy.Application.Operations.WorkOrder.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrder.Commands.UpdateWorkOrder;

/// <summary>
/// <see cref="UpdateWorkOrderCommand"/> handler'ı. <see cref="IWorkOrderService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateWorkOrderCommandHandler
    : IRequestHandler<UpdateWorkOrderCommand, BaseResponse<bool>>
{
    private readonly IWorkOrderService _service;

    public UpdateWorkOrderCommandHandler(IWorkOrderService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateWorkOrderCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
