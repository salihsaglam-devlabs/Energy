using FluentValidation;
using Energy.Shared.Models.V1.IAM.Permission.Requests;

namespace Energy.Application.Modules.IAM.Permission.Validators;

/// <summary>UpdatePermissionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdatePermissionRequestValidator : AbstractValidator<UpdatePermissionRequest>
{
    public UpdatePermissionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Module).NotEmpty();
        RuleFor(x => x.Action).NotEmpty();
        RuleFor(x => x.DisplayNameKey).NotEmpty();
    }
}
