using Energy.Application.Modules.Operations.WorkOrderChecklistItem.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderChecklistItem.Queries.GetWorkOrderChecklistItemList;

/// <summary>
/// <see cref="GetWorkOrderChecklistItemListQuery"/> handler'ı. <see cref="IWorkOrderChecklistItemService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderChecklistItemListQueryHandler
    : IRequestHandler<GetWorkOrderChecklistItemListQuery, BaseResponse<PaginatedResponse<WorkOrderChecklistItemListResponse>>>
{
    private readonly IWorkOrderChecklistItemService _service;

    public GetWorkOrderChecklistItemListQueryHandler(IWorkOrderChecklistItemService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<WorkOrderChecklistItemListResponse>>> Handle(
        GetWorkOrderChecklistItemListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
