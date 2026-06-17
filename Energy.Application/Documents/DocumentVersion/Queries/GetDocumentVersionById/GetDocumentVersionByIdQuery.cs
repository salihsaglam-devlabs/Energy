using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentVersion.Queries.GetDocumentVersionById;

/// <summary>Kimliğe göre DocumentVersion detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetDocumentVersionByIdQuery(Guid Id)
    : IRequest<BaseResponse<DocumentVersionDetailResponse>>;
