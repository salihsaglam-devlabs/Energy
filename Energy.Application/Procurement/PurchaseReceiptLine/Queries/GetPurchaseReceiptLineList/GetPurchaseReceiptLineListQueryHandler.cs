using Energy.Application.Procurement.PurchaseReceiptLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Responses;
using MediatR;

namespace Energy.Application.Procurement.PurchaseReceiptLine.Queries.GetPurchaseReceiptLineList;

/// <summary>
/// <see cref="GetPurchaseReceiptLineListQuery"/> handler'ı. <see cref="IPurchaseReceiptLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetPurchaseReceiptLineListQueryHandler
    : IRequestHandler<GetPurchaseReceiptLineListQuery, BaseResponse<PaginatedResponse<PurchaseReceiptLineListResponse>>>
{
    private readonly IPurchaseReceiptLineService _service;

    public GetPurchaseReceiptLineListQueryHandler(IPurchaseReceiptLineService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<PurchaseReceiptLineListResponse>>> Handle(
        GetPurchaseReceiptLineListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
