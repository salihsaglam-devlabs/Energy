using Energy.Application.Operations.WorkOrder.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrder.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrder.Queries.GetWorkOrderList;

/// <summary>
/// <see cref="GetWorkOrderListQuery"/> handler'ı. <see cref="IWorkOrderService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderListQueryHandler
    : IRequestHandler<GetWorkOrderListQuery, BaseResponse<PaginatedResponse<WorkOrderListResponse>>>
{
    private readonly IWorkOrderService _service;

    public GetWorkOrderListQueryHandler(IWorkOrderService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<WorkOrderListResponse>>> Handle(
        GetWorkOrderListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
