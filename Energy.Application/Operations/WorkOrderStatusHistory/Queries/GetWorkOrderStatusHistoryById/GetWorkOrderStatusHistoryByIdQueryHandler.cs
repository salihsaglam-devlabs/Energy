using Energy.Application.Operations.WorkOrderStatusHistory.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderStatusHistory.Queries.GetWorkOrderStatusHistoryById;

/// <summary>
/// <see cref="GetWorkOrderStatusHistoryByIdQuery"/> handler'ı. <see cref="IWorkOrderStatusHistoryService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderStatusHistoryByIdQueryHandler
    : IRequestHandler<GetWorkOrderStatusHistoryByIdQuery, BaseResponse<WorkOrderStatusHistoryDetailResponse>>
{
    private readonly IWorkOrderStatusHistoryService _service;

    public GetWorkOrderStatusHistoryByIdQueryHandler(IWorkOrderStatusHistoryService service)
        => _service = service;

    public Task<BaseResponse<WorkOrderStatusHistoryDetailResponse>> Handle(
        GetWorkOrderStatusHistoryByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
