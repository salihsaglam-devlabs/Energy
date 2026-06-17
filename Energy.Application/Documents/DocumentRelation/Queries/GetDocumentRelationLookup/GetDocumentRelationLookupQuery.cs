using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentRelation.Queries.GetDocumentRelationLookup;

/// <summary>DocumentRelation lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetDocumentRelationLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<DocumentRelationLookupResponse>>>;
