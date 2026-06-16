using Energy.Application.Operations.WorkOrderMaterialUsage.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderMaterialUsage.Queries.GetWorkOrderMaterialUsageList;

/// <summary>
/// <see cref="GetWorkOrderMaterialUsageListQuery"/> handler'ı. <see cref="IWorkOrderMaterialUsageService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderMaterialUsageListQueryHandler
    : IRequestHandler<GetWorkOrderMaterialUsageListQuery, BaseResponse<PaginatedResponse<WorkOrderMaterialUsageListResponse>>>
{
    private readonly IWorkOrderMaterialUsageService _service;

    public GetWorkOrderMaterialUsageListQueryHandler(IWorkOrderMaterialUsageService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<WorkOrderMaterialUsageListResponse>>> Handle(
        GetWorkOrderMaterialUsageListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
