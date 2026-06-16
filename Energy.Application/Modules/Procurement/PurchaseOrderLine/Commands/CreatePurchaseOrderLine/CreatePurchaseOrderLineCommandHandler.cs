using Energy.Application.Modules.Procurement.PurchaseOrderLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseOrderLine.Commands.CreatePurchaseOrderLine;

/// <summary>
/// <see cref="CreatePurchaseOrderLineCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IPurchaseOrderLineService"/>'i orkestre eder.
/// </summary>
public sealed class CreatePurchaseOrderLineCommandHandler
    : IRequestHandler<CreatePurchaseOrderLineCommand, BaseResponse<Guid>>
{
    private readonly IPurchaseOrderLineService _service;

    public CreatePurchaseOrderLineCommandHandler(IPurchaseOrderLineService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreatePurchaseOrderLineCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
