using Energy.Application.Modules.Inventory.StockIssueAllocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockIssueAllocation.Queries.GetStockIssueAllocationById;

/// <summary>
/// <see cref="GetStockIssueAllocationByIdQuery"/> handler'ı. <see cref="IStockIssueAllocationService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockIssueAllocationByIdQueryHandler
    : IRequestHandler<GetStockIssueAllocationByIdQuery, BaseResponse<StockIssueAllocationDetailResponse>>
{
    private readonly IStockIssueAllocationService _service;

    public GetStockIssueAllocationByIdQueryHandler(IStockIssueAllocationService service)
        => _service = service;

    public Task<BaseResponse<StockIssueAllocationDetailResponse>> Handle(
        GetStockIssueAllocationByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
