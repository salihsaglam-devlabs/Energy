using Energy.Application.Procurement.PurchaseReceipt.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Responses;
using MediatR;

namespace Energy.Application.Procurement.PurchaseReceipt.Queries.GetPurchaseReceiptLookup;

/// <summary>
/// <see cref="GetPurchaseReceiptLookupQuery"/> handler'ı. <see cref="IPurchaseReceiptLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetPurchaseReceiptLookupQueryHandler
    : IRequestHandler<GetPurchaseReceiptLookupQuery, BaseResponse<IReadOnlyList<PurchaseReceiptLookupResponse>>>
{
    private readonly IPurchaseReceiptLookupService _lookup;

    public GetPurchaseReceiptLookupQueryHandler(IPurchaseReceiptLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<PurchaseReceiptLookupResponse>>> Handle(
        GetPurchaseReceiptLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
