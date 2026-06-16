using Energy.Application.Modules.Procurement.PurchaseOrder.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseOrder.Commands.CreatePurchaseOrder;

/// <summary>
/// <see cref="CreatePurchaseOrderCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IPurchaseOrderService"/>'i orkestre eder.
/// </summary>
public sealed class CreatePurchaseOrderCommandHandler
    : IRequestHandler<CreatePurchaseOrderCommand, BaseResponse<Guid>>
{
    private readonly IPurchaseOrderService _service;

    public CreatePurchaseOrderCommandHandler(IPurchaseOrderService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreatePurchaseOrderCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}

