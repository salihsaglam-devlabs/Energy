using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeOption.Queries.GetMaterialAttributeOptionList;

/// <summary>Sayfalanmış MaterialAttributeOption listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetMaterialAttributeOptionListQuery(GetMaterialAttributeOptionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<MaterialAttributeOptionListResponse>>>;
