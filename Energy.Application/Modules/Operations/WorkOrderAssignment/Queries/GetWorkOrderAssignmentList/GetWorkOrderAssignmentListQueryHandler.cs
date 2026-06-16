using Energy.Application.Modules.Operations.WorkOrderAssignment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderAssignment.Queries.GetWorkOrderAssignmentList;

/// <summary>
/// <see cref="GetWorkOrderAssignmentListQuery"/> handler'ı. <see cref="IWorkOrderAssignmentService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderAssignmentListQueryHandler
    : IRequestHandler<GetWorkOrderAssignmentListQuery, BaseResponse<PaginatedResponse<WorkOrderAssignmentListResponse>>>
{
    private readonly IWorkOrderAssignmentService _service;

    public GetWorkOrderAssignmentListQueryHandler(IWorkOrderAssignmentService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<WorkOrderAssignmentListResponse>>> Handle(
        GetWorkOrderAssignmentListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
