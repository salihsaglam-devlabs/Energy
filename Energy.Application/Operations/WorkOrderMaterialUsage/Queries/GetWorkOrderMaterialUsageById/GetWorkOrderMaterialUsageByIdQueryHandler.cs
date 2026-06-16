using Energy.Application.Operations.WorkOrderMaterialUsage.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderMaterialUsage.Queries.GetWorkOrderMaterialUsageById;

/// <summary>
/// <see cref="GetWorkOrderMaterialUsageByIdQuery"/> handler'ı. <see cref="IWorkOrderMaterialUsageService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderMaterialUsageByIdQueryHandler
    : IRequestHandler<GetWorkOrderMaterialUsageByIdQuery, BaseResponse<WorkOrderMaterialUsageDetailResponse>>
{
    private readonly IWorkOrderMaterialUsageService _service;

    public GetWorkOrderMaterialUsageByIdQueryHandler(IWorkOrderMaterialUsageService service)
        => _service = service;

    public Task<BaseResponse<WorkOrderMaterialUsageDetailResponse>> Handle(
        GetWorkOrderMaterialUsageByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
