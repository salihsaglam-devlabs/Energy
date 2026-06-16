using Energy.Application.ProgressPayments.ProgressPayment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Responses;
using MediatR;

namespace Energy.Application.ProgressPayments.ProgressPayment.Queries.GetProgressPaymentList;

/// <summary>
/// <see cref="GetProgressPaymentListQuery"/> handler'ı. <see cref="IProgressPaymentService"/>'i orkestre eder.
/// </summary>
public sealed class GetProgressPaymentListQueryHandler
    : IRequestHandler<GetProgressPaymentListQuery, BaseResponse<PaginatedResponse<ProgressPaymentListResponse>>>
{
    private readonly IProgressPaymentService _service;

    public GetProgressPaymentListQueryHandler(IProgressPaymentService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ProgressPaymentListResponse>>> Handle(
        GetProgressPaymentListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
