using FluentValidation;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Requests;

namespace Energy.Application.Modules.Documents.DocumentVersion.Validators;

/// <summary>CreateDocumentVersionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateDocumentVersionRequestValidator : AbstractValidator<CreateDocumentVersionRequest>
{
    public CreateDocumentVersionRequestValidator()
    {
        RuleFor(x => x.DocumentId).NotEmpty();
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.FilePath).NotEmpty();
    }
}
