using Energy.Application.Modules.Procurement.PurchaseOrder.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseOrder.Queries.GetPurchaseOrderList;

/// <summary>
/// <see cref="GetPurchaseOrderListQuery"/> handler'ı. <see cref="IPurchaseOrderService"/>'i orkestre eder.
/// </summary>
public sealed class GetPurchaseOrderListQueryHandler
    : IRequestHandler<GetPurchaseOrderListQuery, BaseResponse<PaginatedResponse<PurchaseOrderListResponse>>>
{
    private readonly IPurchaseOrderService _service;

    public GetPurchaseOrderListQueryHandler(IPurchaseOrderService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<PurchaseOrderListResponse>>> Handle(
        GetPurchaseOrderListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
