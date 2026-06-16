using Energy.Application.Modules.Operations.WorkOrderChecklistItem.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderChecklistItem.Queries.GetWorkOrderChecklistItemById;

/// <summary>
/// <see cref="GetWorkOrderChecklistItemByIdQuery"/> handler'ı. <see cref="IWorkOrderChecklistItemService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderChecklistItemByIdQueryHandler
    : IRequestHandler<GetWorkOrderChecklistItemByIdQuery, BaseResponse<WorkOrderChecklistItemDetailResponse>>
{
    private readonly IWorkOrderChecklistItemService _service;

    public GetWorkOrderChecklistItemByIdQueryHandler(IWorkOrderChecklistItemService service)
        => _service = service;

    public Task<BaseResponse<WorkOrderChecklistItemDetailResponse>> Handle(
        GetWorkOrderChecklistItemByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
