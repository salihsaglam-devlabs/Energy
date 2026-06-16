using Energy.Application.Procurement.PurchaseReceipt.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Responses;
using MediatR;

namespace Energy.Application.Procurement.PurchaseReceipt.Queries.GetPurchaseReceiptList;

/// <summary>
/// <see cref="GetPurchaseReceiptListQuery"/> handler'ı. <see cref="IPurchaseReceiptService"/>'i orkestre eder.
/// </summary>
public sealed class GetPurchaseReceiptListQueryHandler
    : IRequestHandler<GetPurchaseReceiptListQuery, BaseResponse<PaginatedResponse<PurchaseReceiptListResponse>>>
{
    private readonly IPurchaseReceiptService _service;

    public GetPurchaseReceiptListQueryHandler(IPurchaseReceiptService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<PurchaseReceiptListResponse>>> Handle(
        GetPurchaseReceiptListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
