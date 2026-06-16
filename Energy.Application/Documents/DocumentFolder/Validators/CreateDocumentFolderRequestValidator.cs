using FluentValidation;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Requests;

namespace Energy.Application.Documents.DocumentFolder.Validators;

/// <summary>CreateDocumentFolderRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateDocumentFolderRequestValidator : AbstractValidator<CreateDocumentFolderRequest>
{
    public CreateDocumentFolderRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
