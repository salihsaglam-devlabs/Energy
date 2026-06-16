using Energy.Application.Modules.Finance.Payable.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payable.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Payable.Queries.GetPayableById;

/// <summary>
/// <see cref="GetPayableByIdQuery"/> handler'ı. <see cref="IPayableService"/>'i orkestre eder.
/// </summary>
public sealed class GetPayableByIdQueryHandler
    : IRequestHandler<GetPayableByIdQuery, BaseResponse<PayableDetailResponse>>
{
    private readonly IPayableService _service;

    public GetPayableByIdQueryHandler(IPayableService service)
        => _service = service;

    public Task<BaseResponse<PayableDetailResponse>> Handle(
        GetPayableByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
