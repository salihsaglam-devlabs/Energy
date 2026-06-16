using Energy.Application.Modules.Operations.WorkOrderChecklistItem.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderChecklistItem.Commands.DeleteWorkOrderChecklistItem;

/// <summary>
/// <see cref="DeleteWorkOrderChecklistItemCommand"/> handler'ı. <see cref="IWorkOrderChecklistItemService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteWorkOrderChecklistItemCommandHandler
    : IRequestHandler<DeleteWorkOrderChecklistItemCommand, BaseResponse<bool>>
{
    private readonly IWorkOrderChecklistItemService _service;

    public DeleteWorkOrderChecklistItemCommandHandler(IWorkOrderChecklistItemService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteWorkOrderChecklistItemCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
