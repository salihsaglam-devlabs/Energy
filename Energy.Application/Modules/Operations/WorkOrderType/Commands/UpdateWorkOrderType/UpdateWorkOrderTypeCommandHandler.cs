using Energy.Application.Modules.Operations.WorkOrderType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderType.Commands.UpdateWorkOrderType;

/// <summary>
/// <see cref="UpdateWorkOrderTypeCommand"/> handler'ı. <see cref="IWorkOrderTypeService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateWorkOrderTypeCommandHandler
    : IRequestHandler<UpdateWorkOrderTypeCommand, BaseResponse<bool>>
{
    private readonly IWorkOrderTypeService _service;

    public UpdateWorkOrderTypeCommandHandler(IWorkOrderTypeService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateWorkOrderTypeCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
