using FluentValidation;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Requests;

namespace Energy.Application.Documents.DocumentRelation.Validators;

/// <summary>CreateDocumentRelationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateDocumentRelationRequestValidator : AbstractValidator<CreateDocumentRelationRequest>
{
    public CreateDocumentRelationRequestValidator()
    {
        RuleFor(x => x.DocumentId).NotEmpty();
        RuleFor(x => x.RelatedModule).NotEmpty();
        RuleFor(x => x.RelatedEntityType).NotEmpty();
        RuleFor(x => x.RelatedEntityId).NotEmpty();
    }
}
