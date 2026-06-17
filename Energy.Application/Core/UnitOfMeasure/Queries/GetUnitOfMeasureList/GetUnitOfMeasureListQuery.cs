using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Requests;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Responses;
using MediatR;

namespace Energy.Application.Core.UnitOfMeasure.Queries.GetUnitOfMeasureList;

/// <summary>Sayfalanmış UnitOfMeasure listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetUnitOfMeasureListQuery(GetUnitOfMeasureListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<UnitOfMeasureListResponse>>>;
