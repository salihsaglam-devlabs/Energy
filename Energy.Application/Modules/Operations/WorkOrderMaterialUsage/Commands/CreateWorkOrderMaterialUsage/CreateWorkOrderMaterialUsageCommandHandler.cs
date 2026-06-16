using Energy.Application.Modules.Operations.WorkOrderMaterialUsage.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderMaterialUsage.Commands.CreateWorkOrderMaterialUsage;

/// <summary>
/// <see cref="CreateWorkOrderMaterialUsageCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IWorkOrderMaterialUsageService"/>'i orkestre eder.
/// </summary>
public sealed class CreateWorkOrderMaterialUsageCommandHandler
    : IRequestHandler<CreateWorkOrderMaterialUsageCommand, BaseResponse<Guid>>
{
    private readonly IWorkOrderMaterialUsageService _service;

    public CreateWorkOrderMaterialUsageCommandHandler(IWorkOrderMaterialUsageService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateWorkOrderMaterialUsageCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
