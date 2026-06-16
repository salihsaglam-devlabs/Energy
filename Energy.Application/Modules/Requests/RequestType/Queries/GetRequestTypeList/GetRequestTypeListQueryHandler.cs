using Energy.Application.Modules.Requests.RequestType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestType.Responses;
using MediatR;

namespace Energy.Application.Modules.Requests.RequestType.Queries.GetRequestTypeList;

/// <summary>
/// <see cref="GetRequestTypeListQuery"/> handler'ı. <see cref="IRequestTypeService"/>'i orkestre eder.
/// </summary>
public sealed class GetRequestTypeListQueryHandler
    : IRequestHandler<GetRequestTypeListQuery, BaseResponse<PaginatedResponse<RequestTypeListResponse>>>
{
    private readonly IRequestTypeService _service;

    public GetRequestTypeListQueryHandler(IRequestTypeService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<RequestTypeListResponse>>> Handle(
        GetRequestTypeListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
