using Energy.Application.Modules.Assets.EquipmentAssignment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentAssignment.Commands.UpdateEquipmentAssignment;

/// <summary>
/// <see cref="UpdateEquipmentAssignmentCommand"/> handler'ı. <see cref="IEquipmentAssignmentService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateEquipmentAssignmentCommandHandler
    : IRequestHandler<UpdateEquipmentAssignmentCommand, BaseResponse<bool>>
{
    private readonly IEquipmentAssignmentService _service;

    public UpdateEquipmentAssignmentCommandHandler(IEquipmentAssignmentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateEquipmentAssignmentCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
