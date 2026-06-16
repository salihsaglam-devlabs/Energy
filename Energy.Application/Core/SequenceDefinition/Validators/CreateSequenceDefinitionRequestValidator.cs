using FluentValidation;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Requests;

namespace Energy.Application.Core.SequenceDefinition.Validators;

/// <summary>CreateSequenceDefinitionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateSequenceDefinitionRequestValidator : AbstractValidator<CreateSequenceDefinitionRequest>
{
    public CreateSequenceDefinitionRequestValidator()
    {
        RuleFor(x => x.Module).NotEmpty();
        RuleFor(x => x.EntityType).NotEmpty();
        RuleFor(x => x.Prefix).NotEmpty();
    }
}
