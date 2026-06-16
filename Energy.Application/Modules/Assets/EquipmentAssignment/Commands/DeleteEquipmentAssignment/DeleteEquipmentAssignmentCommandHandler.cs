using Energy.Application.Modules.Assets.EquipmentAssignment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentAssignment.Commands.DeleteEquipmentAssignment;

/// <summary>
/// <see cref="DeleteEquipmentAssignmentCommand"/> handler'ı. <see cref="IEquipmentAssignmentService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteEquipmentAssignmentCommandHandler
    : IRequestHandler<DeleteEquipmentAssignmentCommand, BaseResponse<bool>>
{
    private readonly IEquipmentAssignmentService _service;

    public DeleteEquipmentAssignmentCommandHandler(IEquipmentAssignmentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteEquipmentAssignmentCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
