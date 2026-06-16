using Energy.Application.Finance.FinancialTransaction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialTransaction.Commands.UpdateFinancialTransaction;

/// <summary>
/// <see cref="UpdateFinancialTransactionCommand"/> handler'ı. <see cref="IFinancialTransactionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateFinancialTransactionCommandHandler
    : IRequestHandler<UpdateFinancialTransactionCommand, BaseResponse<bool>>
{
    private readonly IFinancialTransactionService _service;

    public UpdateFinancialTransactionCommandHandler(IFinancialTransactionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateFinancialTransactionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
