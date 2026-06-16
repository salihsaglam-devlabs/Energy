using Energy.Application.Modules.Procurement.PurchaseOrder.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseOrder.Commands.UpdatePurchaseOrder;

/// <summary>
/// <see cref="UpdatePurchaseOrderCommand"/> handler'ı. <see cref="IPurchaseOrderService"/>'i orkestre eder.
/// </summary>
public sealed class UpdatePurchaseOrderCommandHandler
    : IRequestHandler<UpdatePurchaseOrderCommand, BaseResponse<bool>>
{
    private readonly IPurchaseOrderService _service;

    public UpdatePurchaseOrderCommandHandler(IPurchaseOrderService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdatePurchaseOrderCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}

