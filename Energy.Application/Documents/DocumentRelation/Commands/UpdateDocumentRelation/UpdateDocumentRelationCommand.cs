using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Requests;
using MediatR;

namespace Energy.Application.Documents.DocumentRelation.Commands.UpdateDocumentRelation;

/// <summary>Var olan DocumentRelation kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateDocumentRelationCommand(Guid Id, UpdateDocumentRelationRequest Request)
    : IRequest<BaseResponse<bool>>;
