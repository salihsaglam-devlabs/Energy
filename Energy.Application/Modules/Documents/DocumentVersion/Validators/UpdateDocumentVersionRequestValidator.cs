using FluentValidation;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Requests;

namespace Energy.Application.Modules.Documents.DocumentVersion.Validators;

/// <summary>UpdateDocumentVersionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateDocumentVersionRequestValidator : AbstractValidator<UpdateDocumentVersionRequest>
{
    public UpdateDocumentVersionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DocumentId).NotEmpty();
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.FilePath).NotEmpty();
    }
}
