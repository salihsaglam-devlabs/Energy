using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeValue.Queries.GetMaterialAttributeValueList;

/// <summary>Sayfalanmış MaterialAttributeValue listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetMaterialAttributeValueListQuery(GetMaterialAttributeValueListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<MaterialAttributeValueListResponse>>>;
