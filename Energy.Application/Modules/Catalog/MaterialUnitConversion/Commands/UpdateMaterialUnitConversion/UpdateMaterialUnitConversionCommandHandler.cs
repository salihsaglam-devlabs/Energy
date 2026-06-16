using Energy.Application.Modules.Catalog.MaterialUnitConversion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialUnitConversion.Commands.UpdateMaterialUnitConversion;

/// <summary>
/// <see cref="UpdateMaterialUnitConversionCommand"/> handler'ı. <see cref="IMaterialUnitConversionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateMaterialUnitConversionCommandHandler
    : IRequestHandler<UpdateMaterialUnitConversionCommand, BaseResponse<bool>>
{
    private readonly IMaterialUnitConversionService _service;

    public UpdateMaterialUnitConversionCommandHandler(IMaterialUnitConversionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateMaterialUnitConversionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
