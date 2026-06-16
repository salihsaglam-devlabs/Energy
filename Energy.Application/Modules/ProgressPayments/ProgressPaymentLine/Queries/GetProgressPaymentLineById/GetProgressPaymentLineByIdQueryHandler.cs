using Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Queries.GetProgressPaymentLineById;

/// <summary>
/// <see cref="GetProgressPaymentLineByIdQuery"/> handler'ı. <see cref="IProgressPaymentLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetProgressPaymentLineByIdQueryHandler
    : IRequestHandler<GetProgressPaymentLineByIdQuery, BaseResponse<ProgressPaymentLineDetailResponse>>
{
    private readonly IProgressPaymentLineService _service;

    public GetProgressPaymentLineByIdQueryHandler(IProgressPaymentLineService service)
        => _service = service;

    public Task<BaseResponse<ProgressPaymentLineDetailResponse>> Handle(
        GetProgressPaymentLineByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
