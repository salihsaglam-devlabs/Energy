using Energy.Application.Modules.Operations.WorkOrderChecklist.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderChecklist.Queries.GetWorkOrderChecklistById;

/// <summary>
/// <see cref="GetWorkOrderChecklistByIdQuery"/> handler'ı. <see cref="IWorkOrderChecklistService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderChecklistByIdQueryHandler
    : IRequestHandler<GetWorkOrderChecklistByIdQuery, BaseResponse<WorkOrderChecklistDetailResponse>>
{
    private readonly IWorkOrderChecklistService _service;

    public GetWorkOrderChecklistByIdQueryHandler(IWorkOrderChecklistService service)
        => _service = service;

    public Task<BaseResponse<WorkOrderChecklistDetailResponse>> Handle(
        GetWorkOrderChecklistByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
