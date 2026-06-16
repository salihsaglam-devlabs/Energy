using FluentValidation;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Requests;

namespace Energy.Application.Modules.Documents.DocumentPermission.Validators;

/// <summary>CreateDocumentPermissionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateDocumentPermissionRequestValidator : AbstractValidator<CreateDocumentPermissionRequest>
{
    public CreateDocumentPermissionRequestValidator()
    {
        RuleFor(x => x.DocumentId).NotEmpty();
        RuleFor(x => x.AccessType).NotEmpty();
    }
}
