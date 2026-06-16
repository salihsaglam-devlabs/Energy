using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Material.Requests;
using Energy.Shared.Models.V1.Catalog.Material.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.Material.Queries.GetMaterialList;

/// <summary>Sayfalanmış Material listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetMaterialListQuery(GetMaterialListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<MaterialListResponse>>>;
