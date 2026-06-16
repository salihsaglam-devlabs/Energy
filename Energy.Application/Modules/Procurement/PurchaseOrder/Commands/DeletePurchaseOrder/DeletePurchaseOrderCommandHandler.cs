using Energy.Application.Modules.Procurement.PurchaseOrder.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseOrder.Commands.DeletePurchaseOrder;

/// <summary>
/// <see cref="DeletePurchaseOrderCommand"/> handler'ı. <see cref="IPurchaseOrderService"/>'i orkestre eder.
/// </summary>
public sealed class DeletePurchaseOrderCommandHandler
    : IRequestHandler<DeletePurchaseOrderCommand, BaseResponse<bool>>
{
    private readonly IPurchaseOrderService _service;

    public DeletePurchaseOrderCommandHandler(IPurchaseOrderService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeletePurchaseOrderCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}

