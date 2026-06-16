using Energy.Application.Modules.Finance.FinancialTransactionLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.FinancialTransactionLine.Queries.GetFinancialTransactionLineById;

/// <summary>
/// <see cref="GetFinancialTransactionLineByIdQuery"/> handler'ı. <see cref="IFinancialTransactionLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetFinancialTransactionLineByIdQueryHandler
    : IRequestHandler<GetFinancialTransactionLineByIdQuery, BaseResponse<FinancialTransactionLineDetailResponse>>
{
    private readonly IFinancialTransactionLineService _service;

    public GetFinancialTransactionLineByIdQueryHandler(IFinancialTransactionLineService service)
        => _service = service;

    public Task<BaseResponse<FinancialTransactionLineDetailResponse>> Handle(
        GetFinancialTransactionLineByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
