using Energy.Application.Finance.PaymentAllocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Responses;
using MediatR;

namespace Energy.Application.Finance.PaymentAllocation.Queries.GetPaymentAllocationList;

/// <summary>
/// <see cref="GetPaymentAllocationListQuery"/> handler'ı. <see cref="IPaymentAllocationService"/>'i orkestre eder.
/// </summary>
public sealed class GetPaymentAllocationListQueryHandler
    : IRequestHandler<GetPaymentAllocationListQuery, BaseResponse<PaginatedResponse<PaymentAllocationListResponse>>>
{
    private readonly IPaymentAllocationService _service;

    public GetPaymentAllocationListQueryHandler(IPaymentAllocationService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<PaymentAllocationListResponse>>> Handle(
        GetPaymentAllocationListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
