using Energy.Application.Operations.WorkOrderAssignment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderAssignment.Queries.GetWorkOrderAssignmentById;

/// <summary>
/// <see cref="GetWorkOrderAssignmentByIdQuery"/> handler'ı. <see cref="IWorkOrderAssignmentService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderAssignmentByIdQueryHandler
    : IRequestHandler<GetWorkOrderAssignmentByIdQuery, BaseResponse<WorkOrderAssignmentDetailResponse>>
{
    private readonly IWorkOrderAssignmentService _service;

    public GetWorkOrderAssignmentByIdQueryHandler(IWorkOrderAssignmentService service)
        => _service = service;

    public Task<BaseResponse<WorkOrderAssignmentDetailResponse>> Handle(
        GetWorkOrderAssignmentByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
