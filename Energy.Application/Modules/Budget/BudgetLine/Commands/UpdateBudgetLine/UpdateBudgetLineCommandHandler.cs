using Energy.Application.Modules.Budget.BudgetLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Budget.BudgetLine.Commands.UpdateBudgetLine;

/// <summary>
/// <see cref="UpdateBudgetLineCommand"/> handler'ı. <see cref="IBudgetLineService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateBudgetLineCommandHandler
    : IRequestHandler<UpdateBudgetLineCommand, BaseResponse<bool>>
{
    private readonly IBudgetLineService _service;

    public UpdateBudgetLineCommandHandler(IBudgetLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateBudgetLineCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
