using Energy.Application.Operations.WorkOrderMaterialUsage.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderMaterialUsage.Commands.UpdateWorkOrderMaterialUsage;

/// <summary>
/// <see cref="UpdateWorkOrderMaterialUsageCommand"/> handler'ı. <see cref="IWorkOrderMaterialUsageService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateWorkOrderMaterialUsageCommandHandler
    : IRequestHandler<UpdateWorkOrderMaterialUsageCommand, BaseResponse<bool>>
{
    private readonly IWorkOrderMaterialUsageService _service;

    public UpdateWorkOrderMaterialUsageCommandHandler(IWorkOrderMaterialUsageService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateWorkOrderMaterialUsageCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
