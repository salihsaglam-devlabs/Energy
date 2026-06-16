using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Document.Requests;
using Energy.Shared.Models.V1.Documents.Document.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.Document.Queries.GetDocumentList;

/// <summary>Sayfalanmış Document listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetDocumentListQuery(GetDocumentListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<DocumentListResponse>>>;
