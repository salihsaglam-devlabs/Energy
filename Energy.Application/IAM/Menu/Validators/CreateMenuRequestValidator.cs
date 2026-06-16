using FluentValidation;
using Energy.Shared.Models.V1.IAM.Menu.Requests;

namespace Energy.Application.IAM.Menu.Validators;

/// <summary>CreateMenuRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateMenuRequestValidator : AbstractValidator<CreateMenuRequest>
{
    public CreateMenuRequestValidator()
    {
        RuleFor(x => x.NameKey).NotEmpty();
    }
}
