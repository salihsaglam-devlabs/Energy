using Energy.Application.Procurement.PurchaseOrderLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.PurchaseOrderLine.Commands.DeletePurchaseOrderLine;

/// <summary>
/// <see cref="DeletePurchaseOrderLineCommand"/> handler'ı. <see cref="IPurchaseOrderLineService"/>'i orkestre eder.
/// </summary>
public sealed class DeletePurchaseOrderLineCommandHandler
    : IRequestHandler<DeletePurchaseOrderLineCommand, BaseResponse<bool>>
{
    private readonly IPurchaseOrderLineService _service;

    public DeletePurchaseOrderLineCommandHandler(IPurchaseOrderLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeletePurchaseOrderLineCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
