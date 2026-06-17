using FluentValidation;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Requests;

namespace Energy.Application.Catalog.MaterialAttributeValue.Validators;

/// <summary>UpdateMaterialAttributeValueRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateMaterialAttributeValueRequestValidator : AbstractValidator<UpdateMaterialAttributeValueRequest>
{
    public UpdateMaterialAttributeValueRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
        RuleFor(x => x.MaterialAttributeDefinitionId).NotEmpty();
    }
}
