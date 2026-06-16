using Energy.Application.Modules.Organization.ExpenseClaimLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.ExpenseClaimLine.Commands.DeleteExpenseClaimLine;

/// <summary>
/// <see cref="DeleteExpenseClaimLineCommand"/> handler'ı. <see cref="IExpenseClaimLineService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteExpenseClaimLineCommandHandler
    : IRequestHandler<DeleteExpenseClaimLineCommand, BaseResponse<bool>>
{
    private readonly IExpenseClaimLineService _service;

    public DeleteExpenseClaimLineCommandHandler(IExpenseClaimLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteExpenseClaimLineCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
