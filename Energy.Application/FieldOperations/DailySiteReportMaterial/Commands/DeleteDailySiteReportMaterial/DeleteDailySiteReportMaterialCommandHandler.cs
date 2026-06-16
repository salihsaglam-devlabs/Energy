using Energy.Application.FieldOperations.DailySiteReportMaterial.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportMaterial.Commands.DeleteDailySiteReportMaterial;

/// <summary>
/// <see cref="DeleteDailySiteReportMaterialCommand"/> handler'ı. <see cref="IDailySiteReportMaterialService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteDailySiteReportMaterialCommandHandler
    : IRequestHandler<DeleteDailySiteReportMaterialCommand, BaseResponse<bool>>
{
    private readonly IDailySiteReportMaterialService _service;

    public DeleteDailySiteReportMaterialCommandHandler(IDailySiteReportMaterialService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteDailySiteReportMaterialCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
