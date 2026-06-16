using Energy.Application.Modules.Inventory.StockIssueAllocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockIssueAllocation.Queries.GetStockIssueAllocationList;

/// <summary>
/// <see cref="GetStockIssueAllocationListQuery"/> handler'ı. <see cref="IStockIssueAllocationService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockIssueAllocationListQueryHandler
    : IRequestHandler<GetStockIssueAllocationListQuery, BaseResponse<PaginatedResponse<StockIssueAllocationListResponse>>>
{
    private readonly IStockIssueAllocationService _service;

    public GetStockIssueAllocationListQueryHandler(IStockIssueAllocationService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<StockIssueAllocationListResponse>>> Handle(
        GetStockIssueAllocationListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
