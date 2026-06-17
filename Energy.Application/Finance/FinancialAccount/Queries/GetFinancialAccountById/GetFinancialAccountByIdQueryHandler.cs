using Energy.Application.Finance.FinancialAccount.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialAccount.Queries.GetFinancialAccountById;

/// <summary>
/// <see cref="GetFinancialAccountByIdQuery"/> handler'ı. <see cref="IFinancialAccountService"/>'i orkestre eder.
/// </summary>
public sealed class GetFinancialAccountByIdQueryHandler
    : IRequestHandler<GetFinancialAccountByIdQuery, BaseResponse<FinancialAccountDetailResponse>>
{
    private readonly IFinancialAccountService _service;

    public GetFinancialAccountByIdQueryHandler(IFinancialAccountService service)
        => _service = service;

    public Task<BaseResponse<FinancialAccountDetailResponse>> Handle(
        GetFinancialAccountByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
