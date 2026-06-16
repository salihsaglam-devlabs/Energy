using FluentValidation;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Requests;

namespace Energy.Application.Modules.Core.SequenceDefinition.Validators;

/// <summary>UpdateSequenceDefinitionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateSequenceDefinitionRequestValidator : AbstractValidator<UpdateSequenceDefinitionRequest>
{
    public UpdateSequenceDefinitionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Module).NotEmpty();
        RuleFor(x => x.EntityType).NotEmpty();
        RuleFor(x => x.Prefix).NotEmpty();
    }
}
