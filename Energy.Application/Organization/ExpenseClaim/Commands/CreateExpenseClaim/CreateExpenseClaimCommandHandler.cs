using Energy.Application.Organization.ExpenseClaim.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Organization.ExpenseClaim.Commands.CreateExpenseClaim;

/// <summary>
/// <see cref="CreateExpenseClaimCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IExpenseClaimService"/>'i orkestre eder.
/// </summary>
public sealed class CreateExpenseClaimCommandHandler
    : IRequestHandler<CreateExpenseClaimCommand, BaseResponse<Guid>>
{
    private readonly IExpenseClaimService _service;

    public CreateExpenseClaimCommandHandler(IExpenseClaimService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateExpenseClaimCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
