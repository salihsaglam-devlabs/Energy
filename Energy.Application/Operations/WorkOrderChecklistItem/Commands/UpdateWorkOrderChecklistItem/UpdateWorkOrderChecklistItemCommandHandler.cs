using Energy.Application.Operations.WorkOrderChecklistItem.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderChecklistItem.Commands.UpdateWorkOrderChecklistItem;

/// <summary>
/// <see cref="UpdateWorkOrderChecklistItemCommand"/> handler'ı. <see cref="IWorkOrderChecklistItemService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateWorkOrderChecklistItemCommandHandler
    : IRequestHandler<UpdateWorkOrderChecklistItemCommand, BaseResponse<bool>>
{
    private readonly IWorkOrderChecklistItemService _service;

    public UpdateWorkOrderChecklistItemCommandHandler(IWorkOrderChecklistItemService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateWorkOrderChecklistItemCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
