using FluentValidation;

namespace Energy.Application.Localization.Commands.UpsertLocalizationEntry;

public sealed class UpsertLocalizationEntryCommandValidator
    : AbstractValidator<UpsertLocalizationEntryCommand>
{
    public UpsertLocalizationEntryCommandValidator()
    {
        RuleFor(x => x.Request.Key)
            .NotEmpty()
            .MaximumLength(200)
            .Matches("^[A-Za-z0-9_.-]+$")
            .WithMessage("Key may only contain letters, digits, '.', '_' and '-'.");

        RuleFor(x => x.Request.Values)
            .NotNull()
            .Must(values => values.Count > 0)
            .WithMessage("At least one (culture, value) pair must be provided.");

        RuleForEach(x => x.Request.Values)
            .ChildRules(pair =>
            {
                pair.RuleFor(p => p.Key)
                    .NotNull()
                    .MaximumLength(15)
                    .WithMessage("Culture name is too long.");

                pair.RuleFor(p => p.Value)
                    .NotNull();
            });
    }
}

