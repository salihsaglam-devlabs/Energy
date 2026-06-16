using Energy.Application.Modules.Inventory.StockDocumentType.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocumentType.Queries.GetStockDocumentTypeLookup;

/// <summary>
/// <see cref="GetStockDocumentTypeLookupQuery"/> handler'ı. <see cref="IStockDocumentTypeLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockDocumentTypeLookupQueryHandler
    : IRequestHandler<GetStockDocumentTypeLookupQuery, BaseResponse<IReadOnlyList<StockDocumentTypeLookupResponse>>>
{
    private readonly IStockDocumentTypeLookupService _lookup;

    public GetStockDocumentTypeLookupQueryHandler(IStockDocumentTypeLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<StockDocumentTypeLookupResponse>>> Handle(
        GetStockDocumentTypeLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
