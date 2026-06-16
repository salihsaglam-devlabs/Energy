using Energy.Application.Modules.Operations.WorkOrderType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderType.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderType.Queries.GetWorkOrderTypeById;

/// <summary>
/// <see cref="GetWorkOrderTypeByIdQuery"/> handler'ı. <see cref="IWorkOrderTypeService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderTypeByIdQueryHandler
    : IRequestHandler<GetWorkOrderTypeByIdQuery, BaseResponse<WorkOrderTypeDetailResponse>>
{
    private readonly IWorkOrderTypeService _service;

    public GetWorkOrderTypeByIdQueryHandler(IWorkOrderTypeService service)
        => _service = service;

    public Task<BaseResponse<WorkOrderTypeDetailResponse>> Handle(
        GetWorkOrderTypeByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
