using Energy.Application.Finance.Receivable.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Receivable.Responses;
using MediatR;

namespace Energy.Application.Finance.Receivable.Queries.GetReceivableById;

/// <summary>
/// <see cref="GetReceivableByIdQuery"/> handler'ı. <see cref="IReceivableService"/>'i orkestre eder.
/// </summary>
public sealed class GetReceivableByIdQueryHandler
    : IRequestHandler<GetReceivableByIdQuery, BaseResponse<ReceivableDetailResponse>>
{
    private readonly IReceivableService _service;

    public GetReceivableByIdQueryHandler(IReceivableService service)
        => _service = service;

    public Task<BaseResponse<ReceivableDetailResponse>> Handle(
        GetReceivableByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
