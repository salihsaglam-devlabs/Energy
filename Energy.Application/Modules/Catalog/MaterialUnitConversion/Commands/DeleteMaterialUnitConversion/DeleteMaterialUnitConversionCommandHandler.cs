using Energy.Application.Modules.Catalog.MaterialUnitConversion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialUnitConversion.Commands.DeleteMaterialUnitConversion;

/// <summary>
/// <see cref="DeleteMaterialUnitConversionCommand"/> handler'ı. <see cref="IMaterialUnitConversionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteMaterialUnitConversionCommandHandler
    : IRequestHandler<DeleteMaterialUnitConversionCommand, BaseResponse<bool>>
{
    private readonly IMaterialUnitConversionService _service;

    public DeleteMaterialUnitConversionCommandHandler(IMaterialUnitConversionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteMaterialUnitConversionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
