using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Requests;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentRelation.Queries.GetDocumentRelationList;

/// <summary>Sayfalanmış DocumentRelation listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetDocumentRelationListQuery(GetDocumentRelationListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<DocumentRelationListResponse>>>;
