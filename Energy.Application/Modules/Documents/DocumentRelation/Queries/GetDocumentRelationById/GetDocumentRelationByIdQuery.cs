using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentRelation.Queries.GetDocumentRelationById;

/// <summary>Kimliğe göre DocumentRelation detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetDocumentRelationByIdQuery(Guid Id)
    : IRequest<BaseResponse<DocumentRelationDetailResponse>>;
