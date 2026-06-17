using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Brand.Requests;
using Energy.Shared.Models.V1.Catalog.Brand.Responses;
using MediatR;

namespace Energy.Application.Catalog.Brand.Queries.GetBrandList;

/// <summary>Sayfalanmış Brand listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetBrandListQuery(GetBrandListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<BrandListResponse>>>;
