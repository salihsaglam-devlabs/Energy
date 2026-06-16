using FluentValidation;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Requests;

namespace Energy.Application.Modules.Documents.DocumentFolder.Validators;

/// <summary>UpdateDocumentFolderRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateDocumentFolderRequestValidator : AbstractValidator<UpdateDocumentFolderRequest>
{
    public UpdateDocumentFolderRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
