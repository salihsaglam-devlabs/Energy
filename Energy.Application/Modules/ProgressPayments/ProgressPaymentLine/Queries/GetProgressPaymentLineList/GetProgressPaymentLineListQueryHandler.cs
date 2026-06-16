using Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Queries.GetProgressPaymentLineList;

/// <summary>
/// <see cref="GetProgressPaymentLineListQuery"/> handler'ı. <see cref="IProgressPaymentLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetProgressPaymentLineListQueryHandler
    : IRequestHandler<GetProgressPaymentLineListQuery, BaseResponse<PaginatedResponse<ProgressPaymentLineListResponse>>>
{
    private readonly IProgressPaymentLineService _service;

    public GetProgressPaymentLineListQueryHandler(IProgressPaymentLineService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ProgressPaymentLineListResponse>>> Handle(
        GetProgressPaymentLineListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
