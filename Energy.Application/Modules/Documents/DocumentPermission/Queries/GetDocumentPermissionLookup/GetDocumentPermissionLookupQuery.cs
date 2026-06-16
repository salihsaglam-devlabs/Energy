using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentPermission.Queries.GetDocumentPermissionLookup;

/// <summary>DocumentPermission lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetDocumentPermissionLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<DocumentPermissionLookupResponse>>>;
