using Energy.Application.Modules.FieldOperations.DailySiteReportMaterial.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportMaterial.Commands.UpdateDailySiteReportMaterial;

/// <summary>
/// <see cref="UpdateDailySiteReportMaterialCommand"/> handler'ı. <see cref="IDailySiteReportMaterialService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateDailySiteReportMaterialCommandHandler
    : IRequestHandler<UpdateDailySiteReportMaterialCommand, BaseResponse<bool>>
{
    private readonly IDailySiteReportMaterialService _service;

    public UpdateDailySiteReportMaterialCommandHandler(IDailySiteReportMaterialService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateDailySiteReportMaterialCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
