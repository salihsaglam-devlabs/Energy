using Energy.Application.Operations.WorkOrderType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderType.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderType.Queries.GetWorkOrderTypeList;

/// <summary>
/// <see cref="GetWorkOrderTypeListQuery"/> handler'ı. <see cref="IWorkOrderTypeService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderTypeListQueryHandler
    : IRequestHandler<GetWorkOrderTypeListQuery, BaseResponse<PaginatedResponse<WorkOrderTypeListResponse>>>
{
    private readonly IWorkOrderTypeService _service;

    public GetWorkOrderTypeListQueryHandler(IWorkOrderTypeService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<WorkOrderTypeListResponse>>> Handle(
        GetWorkOrderTypeListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
