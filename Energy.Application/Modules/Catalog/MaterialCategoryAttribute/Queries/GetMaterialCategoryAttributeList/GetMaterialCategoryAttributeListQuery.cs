using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Queries.GetMaterialCategoryAttributeList;

/// <summary>Sayfalanmış MaterialCategoryAttribute listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetMaterialCategoryAttributeListQuery(GetMaterialCategoryAttributeListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<MaterialCategoryAttributeListResponse>>>;
