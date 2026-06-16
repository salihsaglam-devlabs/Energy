using Energy.Application.Operations.WorkOrderChecklist.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderChecklist.Queries.GetWorkOrderChecklistList;

/// <summary>
/// <see cref="GetWorkOrderChecklistListQuery"/> handler'ı. <see cref="IWorkOrderChecklistService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderChecklistListQueryHandler
    : IRequestHandler<GetWorkOrderChecklistListQuery, BaseResponse<PaginatedResponse<WorkOrderChecklistListResponse>>>
{
    private readonly IWorkOrderChecklistService _service;

    public GetWorkOrderChecklistListQueryHandler(IWorkOrderChecklistService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<WorkOrderChecklistListResponse>>> Handle(
        GetWorkOrderChecklistListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
