using Energy.Application.Finance.Payable.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payable.Responses;
using MediatR;

namespace Energy.Application.Finance.Payable.Queries.GetPayableList;

/// <summary>
/// <see cref="GetPayableListQuery"/> handler'ı. <see cref="IPayableService"/>'i orkestre eder.
/// </summary>
public sealed class GetPayableListQueryHandler
    : IRequestHandler<GetPayableListQuery, BaseResponse<PaginatedResponse<PayableListResponse>>>
{
    private readonly IPayableService _service;

    public GetPayableListQueryHandler(IPayableService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<PayableListResponse>>> Handle(
        GetPayableListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
