using Energy.Application.Operations.WorkOrder.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrder.Commands.CreateWorkOrder;

/// <summary>
/// <see cref="CreateWorkOrderCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IWorkOrderService"/>'i orkestre eder.
/// </summary>
public sealed class CreateWorkOrderCommandHandler
    : IRequestHandler<CreateWorkOrderCommand, BaseResponse<Guid>>
{
    private readonly IWorkOrderService _service;

    public CreateWorkOrderCommandHandler(IWorkOrderService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateWorkOrderCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
