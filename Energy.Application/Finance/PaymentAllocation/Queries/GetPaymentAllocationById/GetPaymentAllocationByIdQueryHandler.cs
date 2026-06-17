using Energy.Application.Finance.PaymentAllocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Responses;
using MediatR;

namespace Energy.Application.Finance.PaymentAllocation.Queries.GetPaymentAllocationById;

/// <summary>
/// <see cref="GetPaymentAllocationByIdQuery"/> handler'ı. <see cref="IPaymentAllocationService"/>'i orkestre eder.
/// </summary>
public sealed class GetPaymentAllocationByIdQueryHandler
    : IRequestHandler<GetPaymentAllocationByIdQuery, BaseResponse<PaymentAllocationDetailResponse>>
{
    private readonly IPaymentAllocationService _service;

    public GetPaymentAllocationByIdQueryHandler(IPaymentAllocationService service)
        => _service = service;

    public Task<BaseResponse<PaymentAllocationDetailResponse>> Handle(
        GetPaymentAllocationByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
