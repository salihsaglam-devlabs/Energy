using FluentValidation;
using Energy.Shared.Models.V1.IAM.Permission.Requests;

namespace Energy.Application.Modules.IAM.Permission.Validators;

/// <summary>CreatePermissionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreatePermissionRequestValidator : AbstractValidator<CreatePermissionRequest>
{
    public CreatePermissionRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Module).NotEmpty();
        RuleFor(x => x.Action).NotEmpty();
        RuleFor(x => x.DisplayNameKey).NotEmpty();
    }
}
