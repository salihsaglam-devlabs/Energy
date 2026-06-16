using Energy.Application.Modules.Finance.FinancialTransactionLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.FinancialTransactionLine.Commands.UpdateFinancialTransactionLine;

/// <summary>
/// <see cref="UpdateFinancialTransactionLineCommand"/> handler'ı. <see cref="IFinancialTransactionLineService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateFinancialTransactionLineCommandHandler
    : IRequestHandler<UpdateFinancialTransactionLineCommand, BaseResponse<bool>>
{
    private readonly IFinancialTransactionLineService _service;

    public UpdateFinancialTransactionLineCommandHandler(IFinancialTransactionLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateFinancialTransactionLineCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
