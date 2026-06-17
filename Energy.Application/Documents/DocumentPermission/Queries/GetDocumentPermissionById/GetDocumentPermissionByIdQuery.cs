using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentPermission.Queries.GetDocumentPermissionById;

/// <summary>Kimliğe göre DocumentPermission detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetDocumentPermissionByIdQuery(Guid Id)
    : IRequest<BaseResponse<DocumentPermissionDetailResponse>>;
