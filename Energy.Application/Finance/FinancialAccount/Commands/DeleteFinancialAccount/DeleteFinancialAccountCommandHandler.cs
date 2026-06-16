using Energy.Application.Finance.FinancialAccount.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialAccount.Commands.DeleteFinancialAccount;

/// <summary>
/// <see cref="DeleteFinancialAccountCommand"/> handler'ı. <see cref="IFinancialAccountService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteFinancialAccountCommandHandler
    : IRequestHandler<DeleteFinancialAccountCommand, BaseResponse<bool>>
{
    private readonly IFinancialAccountService _service;

    public DeleteFinancialAccountCommandHandler(IFinancialAccountService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteFinancialAccountCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
