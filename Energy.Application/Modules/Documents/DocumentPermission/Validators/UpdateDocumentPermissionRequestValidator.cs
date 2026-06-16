using FluentValidation;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Requests;

namespace Energy.Application.Modules.Documents.DocumentPermission.Validators;

/// <summary>UpdateDocumentPermissionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateDocumentPermissionRequestValidator : AbstractValidator<UpdateDocumentPermissionRequest>
{
    public UpdateDocumentPermissionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DocumentId).NotEmpty();
        RuleFor(x => x.AccessType).NotEmpty();
    }
}
