using Energy.Application.Modules.Procurement.PurchaseOrder.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseOrder.Queries.GetPurchaseOrderById;

/// <summary>
/// <see cref="GetPurchaseOrderByIdQuery"/> handler'ı. <see cref="IPurchaseOrderService"/>'i orkestre eder.
/// </summary>
public sealed class GetPurchaseOrderByIdQueryHandler
    : IRequestHandler<GetPurchaseOrderByIdQuery, BaseResponse<PurchaseOrderDetailResponse>>
{
    private readonly IPurchaseOrderService _service;

    public GetPurchaseOrderByIdQueryHandler(IPurchaseOrderService service)
        => _service = service;

    public Task<BaseResponse<PurchaseOrderDetailResponse>> Handle(
        GetPurchaseOrderByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
