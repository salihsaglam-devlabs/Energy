using FluentValidation;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Requests;

namespace Energy.Application.Modules.Catalog.MaterialAttributeValue.Validators;

/// <summary>CreateMaterialAttributeValueRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateMaterialAttributeValueRequestValidator : AbstractValidator<CreateMaterialAttributeValueRequest>
{
    public CreateMaterialAttributeValueRequestValidator()
    {
        RuleFor(x => x.MaterialId).NotEmpty();
        RuleFor(x => x.MaterialAttributeDefinitionId).NotEmpty();
    }
}
