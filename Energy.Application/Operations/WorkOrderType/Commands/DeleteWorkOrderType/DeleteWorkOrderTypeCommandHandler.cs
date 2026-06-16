using Energy.Application.Operations.WorkOrderType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderType.Commands.DeleteWorkOrderType;

/// <summary>
/// <see cref="DeleteWorkOrderTypeCommand"/> handler'ı. <see cref="IWorkOrderTypeService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteWorkOrderTypeCommandHandler
    : IRequestHandler<DeleteWorkOrderTypeCommand, BaseResponse<bool>>
{
    private readonly IWorkOrderTypeService _service;

    public DeleteWorkOrderTypeCommandHandler(IWorkOrderTypeService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteWorkOrderTypeCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
