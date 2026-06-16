using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialUnitConversion.Queries.GetMaterialUnitConversionList;

/// <summary>Sayfalanmış MaterialUnitConversion listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetMaterialUnitConversionListQuery(GetMaterialUnitConversionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<MaterialUnitConversionListResponse>>>;
