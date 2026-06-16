using Energy.Application.Inventory.StockIssueAllocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockIssueAllocation.Commands.DeleteStockIssueAllocation;

/// <summary>
/// <see cref="DeleteStockIssueAllocationCommand"/> handler'ı. <see cref="IStockIssueAllocationService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteStockIssueAllocationCommandHandler
    : IRequestHandler<DeleteStockIssueAllocationCommand, BaseResponse<bool>>
{
    private readonly IStockIssueAllocationService _service;

    public DeleteStockIssueAllocationCommandHandler(IStockIssueAllocationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteStockIssueAllocationCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
