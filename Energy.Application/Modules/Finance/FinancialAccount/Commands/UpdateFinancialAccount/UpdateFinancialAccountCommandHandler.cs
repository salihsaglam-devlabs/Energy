using Energy.Application.Modules.Finance.FinancialAccount.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.FinancialAccount.Commands.UpdateFinancialAccount;

/// <summary>
/// <see cref="UpdateFinancialAccountCommand"/> handler'ı. <see cref="IFinancialAccountService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateFinancialAccountCommandHandler
    : IRequestHandler<UpdateFinancialAccountCommand, BaseResponse<bool>>
{
    private readonly IFinancialAccountService _service;

    public UpdateFinancialAccountCommandHandler(IFinancialAccountService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateFinancialAccountCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
