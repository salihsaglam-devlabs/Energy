using Energy.Application.Budget.BudgetLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Budget.BudgetLine.Commands.CreateBudgetLine;

/// <summary>
/// <see cref="CreateBudgetLineCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IBudgetLineService"/>'i orkestre eder.
/// </summary>
public sealed class CreateBudgetLineCommandHandler
    : IRequestHandler<CreateBudgetLineCommand, BaseResponse<Guid>>
{
    private readonly IBudgetLineService _service;

    public CreateBudgetLineCommandHandler(IBudgetLineService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateBudgetLineCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
