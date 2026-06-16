using Energy.Application.Modules.Procurement.PurchaseReceipt.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseReceipt.Queries.GetPurchaseReceiptById;

/// <summary>
/// <see cref="GetPurchaseReceiptByIdQuery"/> handler'ı. <see cref="IPurchaseReceiptService"/>'i orkestre eder.
/// </summary>
public sealed class GetPurchaseReceiptByIdQueryHandler
    : IRequestHandler<GetPurchaseReceiptByIdQuery, BaseResponse<PurchaseReceiptDetailResponse>>
{
    private readonly IPurchaseReceiptService _service;

    public GetPurchaseReceiptByIdQueryHandler(IPurchaseReceiptService service)
        => _service = service;

    public Task<BaseResponse<PurchaseReceiptDetailResponse>> Handle(
        GetPurchaseReceiptByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
