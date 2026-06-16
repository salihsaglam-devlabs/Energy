using Energy.Application.Organization.ExpenseClaimLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Organization.ExpenseClaimLine.Commands.CreateExpenseClaimLine;

/// <summary>
/// <see cref="CreateExpenseClaimLineCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IExpenseClaimLineService"/>'i orkestre eder.
/// </summary>
public sealed class CreateExpenseClaimLineCommandHandler
    : IRequestHandler<CreateExpenseClaimLineCommand, BaseResponse<Guid>>
{
    private readonly IExpenseClaimLineService _service;

    public CreateExpenseClaimLineCommandHandler(IExpenseClaimLineService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateExpenseClaimLineCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
