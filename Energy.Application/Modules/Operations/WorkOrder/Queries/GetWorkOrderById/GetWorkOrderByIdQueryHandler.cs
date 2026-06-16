using Energy.Application.Modules.Operations.WorkOrder.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrder.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrder.Queries.GetWorkOrderById;

/// <summary>
/// <see cref="GetWorkOrderByIdQuery"/> handler'ı. <see cref="IWorkOrderService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderByIdQueryHandler
    : IRequestHandler<GetWorkOrderByIdQuery, BaseResponse<WorkOrderDetailResponse>>
{
    private readonly IWorkOrderService _service;

    public GetWorkOrderByIdQueryHandler(IWorkOrderService service)
        => _service = service;

    public Task<BaseResponse<WorkOrderDetailResponse>> Handle(
        GetWorkOrderByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
