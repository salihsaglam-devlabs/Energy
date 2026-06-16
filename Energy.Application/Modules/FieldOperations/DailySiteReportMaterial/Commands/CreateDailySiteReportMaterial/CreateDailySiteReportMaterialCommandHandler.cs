using Energy.Application.Modules.FieldOperations.DailySiteReportMaterial.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportMaterial.Commands.CreateDailySiteReportMaterial;

/// <summary>
/// <see cref="CreateDailySiteReportMaterialCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IDailySiteReportMaterialService"/>'i orkestre eder.
/// </summary>
public sealed class CreateDailySiteReportMaterialCommandHandler
    : IRequestHandler<CreateDailySiteReportMaterialCommand, BaseResponse<Guid>>
{
    private readonly IDailySiteReportMaterialService _service;

    public CreateDailySiteReportMaterialCommandHandler(IDailySiteReportMaterialService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateDailySiteReportMaterialCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
