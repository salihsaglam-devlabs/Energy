using Energy.Application.Modules.Inventory.StockIssueAllocation.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockIssueAllocation.Queries.GetStockIssueAllocationLookup;

/// <summary>
/// <see cref="GetStockIssueAllocationLookupQuery"/> handler'ı. <see cref="IStockIssueAllocationLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockIssueAllocationLookupQueryHandler
    : IRequestHandler<GetStockIssueAllocationLookupQuery, BaseResponse<IReadOnlyList<StockIssueAllocationLookupResponse>>>
{
    private readonly IStockIssueAllocationLookupService _lookup;

    public GetStockIssueAllocationLookupQueryHandler(IStockIssueAllocationLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<StockIssueAllocationLookupResponse>>> Handle(
        GetStockIssueAllocationLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
