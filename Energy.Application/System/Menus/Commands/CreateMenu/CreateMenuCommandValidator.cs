using FluentValidation;

namespace Energy.Application.System.Menus.Commands.CreateMenu;

public sealed class CreateMenuCommandValidator : AbstractValidator<CreateMenuCommand>
{
    public CreateMenuCommandValidator()
    {
        RuleFor(x => x.Request.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Request.Url).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Request.Icon).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Request.Order).GreaterThanOrEqualTo(0);
    }
}

