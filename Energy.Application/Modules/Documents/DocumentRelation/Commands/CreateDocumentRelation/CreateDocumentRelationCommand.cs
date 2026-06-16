using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Requests;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentRelation.Commands.CreateDocumentRelation;

/// <summary>Yeni DocumentRelation oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateDocumentRelationCommand(CreateDocumentRelationRequest Request)
    : IRequest<BaseResponse<Guid>>;
