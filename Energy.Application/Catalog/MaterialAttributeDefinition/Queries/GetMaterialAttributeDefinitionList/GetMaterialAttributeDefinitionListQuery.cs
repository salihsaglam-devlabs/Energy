using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeDefinition.Queries.GetMaterialAttributeDefinitionList;

/// <summary>Sayfalanmış MaterialAttributeDefinition listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetMaterialAttributeDefinitionListQuery(GetMaterialAttributeDefinitionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<MaterialAttributeDefinitionListResponse>>>;
