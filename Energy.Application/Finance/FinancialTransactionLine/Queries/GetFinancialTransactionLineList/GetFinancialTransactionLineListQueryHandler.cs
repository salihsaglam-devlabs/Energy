using Energy.Application.Finance.FinancialTransactionLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialTransactionLine.Queries.GetFinancialTransactionLineList;

/// <summary>
/// <see cref="GetFinancialTransactionLineListQuery"/> handler'ı. <see cref="IFinancialTransactionLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetFinancialTransactionLineListQueryHandler
    : IRequestHandler<GetFinancialTransactionLineListQuery, BaseResponse<PaginatedResponse<FinancialTransactionLineListResponse>>>
{
    private readonly IFinancialTransactionLineService _service;

    public GetFinancialTransactionLineListQueryHandler(IFinancialTransactionLineService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<FinancialTransactionLineListResponse>>> Handle(
        GetFinancialTransactionLineListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
