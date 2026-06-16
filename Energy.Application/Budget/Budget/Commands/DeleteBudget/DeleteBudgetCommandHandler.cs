using Energy.Application.Budget.Budget.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Budget.Budget.Commands.DeleteBudget;

/// <summary>
/// <see cref="DeleteBudgetCommand"/> handler'ı. <see cref="IBudgetService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteBudgetCommandHandler
    : IRequestHandler<DeleteBudgetCommand, BaseResponse<bool>>
{
    private readonly IBudgetService _service;

    public DeleteBudgetCommandHandler(IBudgetService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteBudgetCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
