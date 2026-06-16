using Energy.Application.Modules.Budget.Budget.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Budget.Budget.Commands.UpdateBudget;

/// <summary>
/// <see cref="UpdateBudgetCommand"/> handler'ı. <see cref="IBudgetService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateBudgetCommandHandler
    : IRequestHandler<UpdateBudgetCommand, BaseResponse<bool>>
{
    private readonly IBudgetService _service;

    public UpdateBudgetCommandHandler(IBudgetService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateBudgetCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
