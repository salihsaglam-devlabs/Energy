using Energy.Application.Modules.Operations.WorkOrderType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderType.Commands.CreateWorkOrderType;

/// <summary>
/// <see cref="CreateWorkOrderTypeCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IWorkOrderTypeService"/>'i orkestre eder.
/// </summary>
public sealed class CreateWorkOrderTypeCommandHandler
    : IRequestHandler<CreateWorkOrderTypeCommand, BaseResponse<Guid>>
{
    private readonly IWorkOrderTypeService _service;

    public CreateWorkOrderTypeCommandHandler(IWorkOrderTypeService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateWorkOrderTypeCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
