using Energy.Application.ProgressPayments.ProgressPaymentDeduction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Responses;
using MediatR;

namespace Energy.Application.ProgressPayments.ProgressPaymentDeduction.Queries.GetProgressPaymentDeductionList;

/// <summary>
/// <see cref="GetProgressPaymentDeductionListQuery"/> handler'ı. <see cref="IProgressPaymentDeductionService"/>'i orkestre eder.
/// </summary>
public sealed class GetProgressPaymentDeductionListQueryHandler
    : IRequestHandler<GetProgressPaymentDeductionListQuery, BaseResponse<PaginatedResponse<ProgressPaymentDeductionListResponse>>>
{
    private readonly IProgressPaymentDeductionService _service;

    public GetProgressPaymentDeductionListQueryHandler(IProgressPaymentDeductionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ProgressPaymentDeductionListResponse>>> Handle(
        GetProgressPaymentDeductionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
