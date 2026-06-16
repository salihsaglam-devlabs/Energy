using FluentValidation;
using Energy.Shared.Models.V1.IAM.Menu.Requests;

namespace Energy.Application.IAM.Menu.Validators;

/// <summary>UpdateMenuRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateMenuRequestValidator : AbstractValidator<UpdateMenuRequest>
{
    public UpdateMenuRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.NameKey).NotEmpty();
    }
}
