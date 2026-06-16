using Energy.Application.Procurement.PurchaseOrderLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Responses;
using MediatR;

namespace Energy.Application.Procurement.PurchaseOrderLine.Queries.GetPurchaseOrderLineList;

/// <summary>
/// <see cref="GetPurchaseOrderLineListQuery"/> handler'ı. <see cref="IPurchaseOrderLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetPurchaseOrderLineListQueryHandler
    : IRequestHandler<GetPurchaseOrderLineListQuery, BaseResponse<PaginatedResponse<PurchaseOrderLineListResponse>>>
{
    private readonly IPurchaseOrderLineService _service;

    public GetPurchaseOrderLineListQueryHandler(IPurchaseOrderLineService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<PurchaseOrderLineListResponse>>> Handle(
        GetPurchaseOrderLineListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
