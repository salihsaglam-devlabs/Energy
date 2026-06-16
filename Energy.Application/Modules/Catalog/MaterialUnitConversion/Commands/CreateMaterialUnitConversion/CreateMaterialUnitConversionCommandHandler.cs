using Energy.Application.Modules.Catalog.MaterialUnitConversion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialUnitConversion.Commands.CreateMaterialUnitConversion;

/// <summary>
/// <see cref="CreateMaterialUnitConversionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IMaterialUnitConversionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateMaterialUnitConversionCommandHandler
    : IRequestHandler<CreateMaterialUnitConversionCommand, BaseResponse<Guid>>
{
    private readonly IMaterialUnitConversionService _service;

    public CreateMaterialUnitConversionCommandHandler(IMaterialUnitConversionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateMaterialUnitConversionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
