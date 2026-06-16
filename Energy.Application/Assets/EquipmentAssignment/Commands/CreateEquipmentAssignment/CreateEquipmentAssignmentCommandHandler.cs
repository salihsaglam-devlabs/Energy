using Energy.Application.Assets.EquipmentAssignment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Assets.EquipmentAssignment.Commands.CreateEquipmentAssignment;

/// <summary>
/// <see cref="CreateEquipmentAssignmentCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IEquipmentAssignmentService"/>'i orkestre eder.
/// </summary>
public sealed class CreateEquipmentAssignmentCommandHandler
    : IRequestHandler<CreateEquipmentAssignmentCommand, BaseResponse<Guid>>
{
    private readonly IEquipmentAssignmentService _service;

    public CreateEquipmentAssignmentCommandHandler(IEquipmentAssignmentService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateEquipmentAssignmentCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
