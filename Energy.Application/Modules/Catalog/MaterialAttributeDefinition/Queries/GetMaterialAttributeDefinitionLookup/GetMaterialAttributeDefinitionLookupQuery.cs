using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialAttributeDefinition.Queries.GetMaterialAttributeDefinitionLookup;

/// <summary>MaterialAttributeDefinition lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetMaterialAttributeDefinitionLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<MaterialAttributeDefinitionLookupResponse>>>;
