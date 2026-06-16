using Energy.Application.Modules.Finance.Payment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payment.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Payment.Queries.GetPaymentById;

/// <summary>
/// <see cref="GetPaymentByIdQuery"/> handler'ı. <see cref="IPaymentService"/>'i orkestre eder.
/// </summary>
public sealed class GetPaymentByIdQueryHandler
    : IRequestHandler<GetPaymentByIdQuery, BaseResponse<PaymentDetailResponse>>
{
    private readonly IPaymentService _service;

    public GetPaymentByIdQueryHandler(IPaymentService service)
        => _service = service;

    public Task<BaseResponse<PaymentDetailResponse>> Handle(
        GetPaymentByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
