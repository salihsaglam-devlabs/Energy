using Energy.Application.Operations.WorkOrderChecklistItem.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderChecklistItem.Commands.CreateWorkOrderChecklistItem;

/// <summary>
/// <see cref="CreateWorkOrderChecklistItemCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IWorkOrderChecklistItemService"/>'i orkestre eder.
/// </summary>
public sealed class CreateWorkOrderChecklistItemCommandHandler
    : IRequestHandler<CreateWorkOrderChecklistItemCommand, BaseResponse<Guid>>
{
    private readonly IWorkOrderChecklistItemService _service;

    public CreateWorkOrderChecklistItemCommandHandler(IWorkOrderChecklistItemService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateWorkOrderChecklistItemCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
