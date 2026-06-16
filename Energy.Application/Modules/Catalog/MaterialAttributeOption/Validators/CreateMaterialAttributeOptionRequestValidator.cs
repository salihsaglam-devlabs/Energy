using FluentValidation;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Requests;

namespace Energy.Application.Modules.Catalog.MaterialAttributeOption.Validators;

/// <summary>CreateMaterialAttributeOptionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateMaterialAttributeOptionRequestValidator : AbstractValidator<CreateMaterialAttributeOptionRequest>
{
    public CreateMaterialAttributeOptionRequestValidator()
    {
        RuleFor(x => x.MaterialAttributeDefinitionId).NotEmpty();
        RuleFor(x => x.Value).NotEmpty();
    }
}
