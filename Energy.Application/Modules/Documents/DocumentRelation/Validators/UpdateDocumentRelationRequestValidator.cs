using FluentValidation;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Requests;

namespace Energy.Application.Modules.Documents.DocumentRelation.Validators;

/// <summary>UpdateDocumentRelationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateDocumentRelationRequestValidator : AbstractValidator<UpdateDocumentRelationRequest>
{
    public UpdateDocumentRelationRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DocumentId).NotEmpty();
        RuleFor(x => x.RelatedModule).NotEmpty();
        RuleFor(x => x.RelatedEntityType).NotEmpty();
        RuleFor(x => x.RelatedEntityId).NotEmpty();
    }
}
