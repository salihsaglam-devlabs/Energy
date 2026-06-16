using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Requests;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentPermission.Queries.GetDocumentPermissionList;

/// <summary>Sayfalanmış DocumentPermission listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetDocumentPermissionListQuery(GetDocumentPermissionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<DocumentPermissionListResponse>>>;
