using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Document.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.Document.Queries.GetDocumentLookup;

/// <summary>Document lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetDocumentLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<DocumentLookupResponse>>>;
