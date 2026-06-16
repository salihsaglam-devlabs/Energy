using Energy.Application.Modules.Budget.BudgetLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Budget.BudgetLine.Commands.DeleteBudgetLine;

/// <summary>
/// <see cref="DeleteBudgetLineCommand"/> handler'ı. <see cref="IBudgetLineService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteBudgetLineCommandHandler
    : IRequestHandler<DeleteBudgetLineCommand, BaseResponse<bool>>
{
    private readonly IBudgetLineService _service;

    public DeleteBudgetLineCommandHandler(IBudgetLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteBudgetLineCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
