using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Requests;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentVersion.Queries.GetDocumentVersionList;

/// <summary>Sayfalanmış DocumentVersion listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetDocumentVersionListQuery(GetDocumentVersionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<DocumentVersionListResponse>>>;
