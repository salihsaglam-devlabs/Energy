using Energy.Application.Operations.WorkOrderStatusHistory.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderStatusHistory.Queries.GetWorkOrderStatusHistoryList;

/// <summary>
/// <see cref="GetWorkOrderStatusHistoryListQuery"/> handler'ı. <see cref="IWorkOrderStatusHistoryService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderStatusHistoryListQueryHandler
    : IRequestHandler<GetWorkOrderStatusHistoryListQuery, BaseResponse<PaginatedResponse<WorkOrderStatusHistoryListResponse>>>
{
    private readonly IWorkOrderStatusHistoryService _service;

    public GetWorkOrderStatusHistoryListQueryHandler(IWorkOrderStatusHistoryService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<WorkOrderStatusHistoryListResponse>>> Handle(
        GetWorkOrderStatusHistoryListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
