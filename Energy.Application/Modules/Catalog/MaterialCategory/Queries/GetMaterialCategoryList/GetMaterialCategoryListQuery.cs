using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategory.Queries.GetMaterialCategoryList;

/// <summary>Sayfalanmış MaterialCategory listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetMaterialCategoryListQuery(GetMaterialCategoryListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<MaterialCategoryListResponse>>>;
