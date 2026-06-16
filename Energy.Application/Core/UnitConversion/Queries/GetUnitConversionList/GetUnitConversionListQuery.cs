using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitConversion.Requests;
using Energy.Shared.Models.V1.Core.UnitConversion.Responses;
using MediatR;

namespace Energy.Application.Core.UnitConversion.Queries.GetUnitConversionList;

/// <summary>Sayfalanmış UnitConversion listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetUnitConversionListQuery(GetUnitConversionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<UnitConversionListResponse>>>;
