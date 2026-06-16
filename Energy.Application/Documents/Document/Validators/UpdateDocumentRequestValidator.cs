using FluentValidation;
using Energy.Shared.Models.V1.Documents.Document.Requests;

namespace Energy.Application.Documents.Document.Validators;

/// <summary>UpdateDocumentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateDocumentRequestValidator : AbstractValidator<UpdateDocumentRequest>
{
    public UpdateDocumentRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
