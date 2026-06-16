using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Requests;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentFolder.Queries.GetDocumentFolderList;

/// <summary>Sayfalanmış DocumentFolder listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetDocumentFolderListQuery(GetDocumentFolderListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<DocumentFolderListResponse>>>;
