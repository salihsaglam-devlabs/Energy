using Energy.Application.Procurement.PurchaseOrder.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Responses;
using MediatR;

namespace Energy.Application.Procurement.PurchaseOrder.Queries.GetPurchaseOrderLookup;

/// <summary>
/// <see cref="GetPurchaseOrderLookupQuery"/> handler'ı. <see cref="IPurchaseOrderLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetPurchaseOrderLookupQueryHandler
    : IRequestHandler<GetPurchaseOrderLookupQuery, BaseResponse<IReadOnlyList<PurchaseOrderLookupResponse>>>
{
    private readonly IPurchaseOrderLookupService _lookup;

    public GetPurchaseOrderLookupQueryHandler(IPurchaseOrderLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<PurchaseOrderLookupResponse>>> Handle(
        GetPurchaseOrderLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
