using Energy.Application.Inventory.StockIssueAllocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockIssueAllocation.Commands.UpdateStockIssueAllocation;

/// <summary>
/// <see cref="UpdateStockIssueAllocationCommand"/> handler'ı. <see cref="IStockIssueAllocationService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateStockIssueAllocationCommandHandler
    : IRequestHandler<UpdateStockIssueAllocationCommand, BaseResponse<bool>>
{
    private readonly IStockIssueAllocationService _service;

    public UpdateStockIssueAllocationCommandHandler(IStockIssueAllocationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateStockIssueAllocationCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
