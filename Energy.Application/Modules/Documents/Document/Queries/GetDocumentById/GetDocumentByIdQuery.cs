using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Document.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.Document.Queries.GetDocumentById;

/// <summary>Kimliğe göre Document detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetDocumentByIdQuery(Guid Id)
    : IRequest<BaseResponse<DocumentDetailResponse>>;
