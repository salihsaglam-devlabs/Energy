using Energy.Application.Finance.Payment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payment.Responses;
using MediatR;

namespace Energy.Application.Finance.Payment.Queries.GetPaymentList;

/// <summary>
/// <see cref="GetPaymentListQuery"/> handler'ı. <see cref="IPaymentService"/>'i orkestre eder.
/// </summary>
public sealed class GetPaymentListQueryHandler
    : IRequestHandler<GetPaymentListQuery, BaseResponse<PaginatedResponse<PaymentListResponse>>>
{
    private readonly IPaymentService _service;

    public GetPaymentListQueryHandler(IPaymentService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<PaymentListResponse>>> Handle(
        GetPaymentListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
