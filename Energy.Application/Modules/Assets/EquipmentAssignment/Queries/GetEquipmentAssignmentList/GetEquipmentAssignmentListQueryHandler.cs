using Energy.Application.Modules.Assets.EquipmentAssignment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Responses;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentAssignment.Queries.GetEquipmentAssignmentList;

/// <summary>
/// <see cref="GetEquipmentAssignmentListQuery"/> handler'ı. <see cref="IEquipmentAssignmentService"/>'i orkestre eder.
/// </summary>
public sealed class GetEquipmentAssignmentListQueryHandler
    : IRequestHandler<GetEquipmentAssignmentListQuery, BaseResponse<PaginatedResponse<EquipmentAssignmentListResponse>>>
{
    private readonly IEquipmentAssignmentService _service;

    public GetEquipmentAssignmentListQueryHandler(IEquipmentAssignmentService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<EquipmentAssignmentListResponse>>> Handle(
        GetEquipmentAssignmentListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
