using Energy.Application.Catalog.MaterialUnitConversion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialUnitConversion.Queries.GetMaterialUnitConversionById;

/// <summary>
/// <see cref="GetMaterialUnitConversionByIdQuery"/> handler'ı. <see cref="IMaterialUnitConversionService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialUnitConversionByIdQueryHandler
    : IRequestHandler<GetMaterialUnitConversionByIdQuery, BaseResponse<MaterialUnitConversionDetailResponse>>
{
    private readonly IMaterialUnitConversionService _service;

    public GetMaterialUnitConversionByIdQueryHandler(IMaterialUnitConversionService service)
        => _service = service;

    public Task<BaseResponse<MaterialUnitConversionDetailResponse>> Handle(
        GetMaterialUnitConversionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
