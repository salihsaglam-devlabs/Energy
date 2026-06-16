using Energy.Application.Modules.Inventory.StockIssueAllocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockIssueAllocation.Commands.CreateStockIssueAllocation;

/// <summary>
/// <see cref="CreateStockIssueAllocationCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IStockIssueAllocationService"/>'i orkestre eder.
/// </summary>
public sealed class CreateStockIssueAllocationCommandHandler
    : IRequestHandler<CreateStockIssueAllocationCommand, BaseResponse<Guid>>
{
    private readonly IStockIssueAllocationService _service;

    public CreateStockIssueAllocationCommandHandler(IStockIssueAllocationService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateStockIssueAllocationCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
