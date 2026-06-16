using Energy.Application.Operations.WorkOrderMaterialUsage.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderMaterialUsage.Commands.DeleteWorkOrderMaterialUsage;

/// <summary>
/// <see cref="DeleteWorkOrderMaterialUsageCommand"/> handler'ı. <see cref="IWorkOrderMaterialUsageService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteWorkOrderMaterialUsageCommandHandler
    : IRequestHandler<DeleteWorkOrderMaterialUsageCommand, BaseResponse<bool>>
{
    private readonly IWorkOrderMaterialUsageService _service;

    public DeleteWorkOrderMaterialUsageCommandHandler(IWorkOrderMaterialUsageService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteWorkOrderMaterialUsageCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
