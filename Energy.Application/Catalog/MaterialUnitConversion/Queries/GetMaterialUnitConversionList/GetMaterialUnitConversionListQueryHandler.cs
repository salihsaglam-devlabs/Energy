using Energy.Application.Catalog.MaterialUnitConversion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialUnitConversion.Queries.GetMaterialUnitConversionList;

/// <summary>
/// <see cref="GetMaterialUnitConversionListQuery"/> handler'ı. <see cref="IMaterialUnitConversionService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialUnitConversionListQueryHandler
    : IRequestHandler<GetMaterialUnitConversionListQuery, BaseResponse<PaginatedResponse<MaterialUnitConversionListResponse>>>
{
    private readonly IMaterialUnitConversionService _service;

    public GetMaterialUnitConversionListQueryHandler(IMaterialUnitConversionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<MaterialUnitConversionListResponse>>> Handle(
        GetMaterialUnitConversionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
