using Energy.Application.Finance.FinancialTransactionLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialTransactionLine.Commands.DeleteFinancialTransactionLine;

/// <summary>
/// <see cref="DeleteFinancialTransactionLineCommand"/> handler'ı. <see cref="IFinancialTransactionLineService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteFinancialTransactionLineCommandHandler
    : IRequestHandler<DeleteFinancialTransactionLineCommand, BaseResponse<bool>>
{
    private readonly IFinancialTransactionLineService _service;

    public DeleteFinancialTransactionLineCommandHandler(IFinancialTransactionLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteFinancialTransactionLineCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
