using Energy.Application.Modules.Budget.Budget.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Budget.Budget.Commands.CreateBudget;

/// <summary>
/// <see cref="CreateBudgetCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IBudgetService"/>'i orkestre eder.
/// </summary>
public sealed class CreateBudgetCommandHandler
    : IRequestHandler<CreateBudgetCommand, BaseResponse<Guid>>
{
    private readonly IBudgetService _service;

    public CreateBudgetCommandHandler(IBudgetService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateBudgetCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
