using Energy.Application.Finance.Receivable.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Receivable.Responses;
using MediatR;

namespace Energy.Application.Finance.Receivable.Queries.GetReceivableList;

/// <summary>
/// <see cref="GetReceivableListQuery"/> handler'ı. <see cref="IReceivableService"/>'i orkestre eder.
/// </summary>
public sealed class GetReceivableListQueryHandler
    : IRequestHandler<GetReceivableListQuery, BaseResponse<PaginatedResponse<ReceivableListResponse>>>
{
    private readonly IReceivableService _service;

    public GetReceivableListQueryHandler(IReceivableService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ReceivableListResponse>>> Handle(
        GetReceivableListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
