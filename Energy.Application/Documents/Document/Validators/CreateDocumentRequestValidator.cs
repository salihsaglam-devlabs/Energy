using FluentValidation;
using Energy.Shared.Models.V1.Documents.Document.Requests;

namespace Energy.Application.Documents.Document.Validators;

/// <summary>CreateDocumentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateDocumentRequestValidator : AbstractValidator<CreateDocumentRequest>
{
    public CreateDocumentRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
