using Energy.Application.Modules.Organization.ExpenseClaim.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.ExpenseClaim.Commands.UpdateExpenseClaim;

/// <summary>
/// <see cref="UpdateExpenseClaimCommand"/> handler'ı. <see cref="IExpenseClaimService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateExpenseClaimCommandHandler
    : IRequestHandler<UpdateExpenseClaimCommand, BaseResponse<bool>>
{
    private readonly IExpenseClaimService _service;

    public UpdateExpenseClaimCommandHandler(IExpenseClaimService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateExpenseClaimCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
