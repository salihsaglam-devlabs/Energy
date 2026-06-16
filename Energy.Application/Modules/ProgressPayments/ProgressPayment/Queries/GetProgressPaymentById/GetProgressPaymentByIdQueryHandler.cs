using Energy.Application.Modules.ProgressPayments.ProgressPayment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPayment.Queries.GetProgressPaymentById;

/// <summary>
/// <see cref="GetProgressPaymentByIdQuery"/> handler'ı. <see cref="IProgressPaymentService"/>'i orkestre eder.
/// </summary>
public sealed class GetProgressPaymentByIdQueryHandler
    : IRequestHandler<GetProgressPaymentByIdQuery, BaseResponse<ProgressPaymentDetailResponse>>
{
    private readonly IProgressPaymentService _service;

    public GetProgressPaymentByIdQueryHandler(IProgressPaymentService service)
        => _service = service;

    public Task<BaseResponse<ProgressPaymentDetailResponse>> Handle(
        GetProgressPaymentByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
