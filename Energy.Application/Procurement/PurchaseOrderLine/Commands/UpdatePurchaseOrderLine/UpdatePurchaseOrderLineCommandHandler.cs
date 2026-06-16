using Energy.Application.Procurement.PurchaseOrderLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.PurchaseOrderLine.Commands.UpdatePurchaseOrderLine;

/// <summary>
/// <see cref="UpdatePurchaseOrderLineCommand"/> handler'ı. <see cref="IPurchaseOrderLineService"/>'i orkestre eder.
/// </summary>
public sealed class UpdatePurchaseOrderLineCommandHandler
    : IRequestHandler<UpdatePurchaseOrderLineCommand, BaseResponse<bool>>
{
    private readonly IPurchaseOrderLineService _service;

    public UpdatePurchaseOrderLineCommandHandler(IPurchaseOrderLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdatePurchaseOrderLineCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
