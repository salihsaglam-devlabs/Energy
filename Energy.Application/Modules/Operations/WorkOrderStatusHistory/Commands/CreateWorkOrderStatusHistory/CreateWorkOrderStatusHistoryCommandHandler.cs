using Energy.Application.Modules.Operations.WorkOrderStatusHistory.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderStatusHistory.Commands.CreateWorkOrderStatusHistory;

/// <summary>
/// <see cref="CreateWorkOrderStatusHistoryCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IWorkOrderStatusHistoryService"/>'i orkestre eder.
/// </summary>
public sealed class CreateWorkOrderStatusHistoryCommandHandler
    : IRequestHandler<CreateWorkOrderStatusHistoryCommand, BaseResponse<Guid>>
{
    private readonly IWorkOrderStatusHistoryService _service;

    public CreateWorkOrderStatusHistoryCommandHandler(IWorkOrderStatusHistoryService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateWorkOrderStatusHistoryCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
