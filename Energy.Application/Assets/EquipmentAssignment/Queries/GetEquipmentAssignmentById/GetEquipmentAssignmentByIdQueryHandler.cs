using Energy.Application.Assets.EquipmentAssignment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Responses;
using MediatR;

namespace Energy.Application.Assets.EquipmentAssignment.Queries.GetEquipmentAssignmentById;

/// <summary>
/// <see cref="GetEquipmentAssignmentByIdQuery"/> handler'ı. <see cref="IEquipmentAssignmentService"/>'i orkestre eder.
/// </summary>
public sealed class GetEquipmentAssignmentByIdQueryHandler
    : IRequestHandler<GetEquipmentAssignmentByIdQuery, BaseResponse<EquipmentAssignmentDetailResponse>>
{
    private readonly IEquipmentAssignmentService _service;

    public GetEquipmentAssignmentByIdQueryHandler(IEquipmentAssignmentService service)
        => _service = service;

    public Task<BaseResponse<EquipmentAssignmentDetailResponse>> Handle(
        GetEquipmentAssignmentByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
