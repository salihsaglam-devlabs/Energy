using Energy.Application.ProgressPayments.ProgressPaymentDeduction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Responses;
using MediatR;

namespace Energy.Application.ProgressPayments.ProgressPaymentDeduction.Queries.GetProgressPaymentDeductionById;

/// <summary>
/// <see cref="GetProgressPaymentDeductionByIdQuery"/> handler'ı. <see cref="IProgressPaymentDeductionService"/>'i orkestre eder.
/// </summary>
public sealed class GetProgressPaymentDeductionByIdQueryHandler
    : IRequestHandler<GetProgressPaymentDeductionByIdQuery, BaseResponse<ProgressPaymentDeductionDetailResponse>>
{
    private readonly IProgressPaymentDeductionService _service;

    public GetProgressPaymentDeductionByIdQueryHandler(IProgressPaymentDeductionService service)
        => _service = service;

    public Task<BaseResponse<ProgressPaymentDeductionDetailResponse>> Handle(
        GetProgressPaymentDeductionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
