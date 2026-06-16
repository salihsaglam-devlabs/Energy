using Energy.Application.Modules.Organization.ExpenseClaim.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.ExpenseClaim.Commands.DeleteExpenseClaim;

/// <summary>
/// <see cref="DeleteExpenseClaimCommand"/> handler'ı. <see cref="IExpenseClaimService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteExpenseClaimCommandHandler
    : IRequestHandler<DeleteExpenseClaimCommand, BaseResponse<bool>>
{
    private readonly IExpenseClaimService _service;

    public DeleteExpenseClaimCommandHandler(IExpenseClaimService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteExpenseClaimCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
