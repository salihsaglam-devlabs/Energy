using Energy.Application.Modules.Procurement.PurchaseOrderLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseOrderLine.Queries.GetPurchaseOrderLineById;

/// <summary>
/// <see cref="GetPurchaseOrderLineByIdQuery"/> handler'ı. <see cref="IPurchaseOrderLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetPurchaseOrderLineByIdQueryHandler
    : IRequestHandler<GetPurchaseOrderLineByIdQuery, BaseResponse<PurchaseOrderLineDetailResponse>>
{
    private readonly IPurchaseOrderLineService _service;

    public GetPurchaseOrderLineByIdQueryHandler(IPurchaseOrderLineService service)
        => _service = service;

    public Task<BaseResponse<PurchaseOrderLineDetailResponse>> Handle(
        GetPurchaseOrderLineByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
