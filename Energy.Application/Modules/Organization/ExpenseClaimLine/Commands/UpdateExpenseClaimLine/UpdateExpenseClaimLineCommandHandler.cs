using Energy.Application.Modules.Organization.ExpenseClaimLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.ExpenseClaimLine.Commands.UpdateExpenseClaimLine;

/// <summary>
/// <see cref="UpdateExpenseClaimLineCommand"/> handler'ı. <see cref="IExpenseClaimLineService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateExpenseClaimLineCommandHandler
    : IRequestHandler<UpdateExpenseClaimLineCommand, BaseResponse<bool>>
{
    private readonly IExpenseClaimLineService _service;

    public UpdateExpenseClaimLineCommandHandler(IExpenseClaimLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateExpenseClaimLineCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
