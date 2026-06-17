using FluentValidation;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Requests;

namespace Energy.Application.Catalog.MaterialCategoryAttribute.Validators;

/// <summary>CreateMaterialCategoryAttributeRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateMaterialCategoryAttributeRequestValidator : AbstractValidator<CreateMaterialCategoryAttributeRequest>
{
    public CreateMaterialCategoryAttributeRequestValidator()
    {
        RuleFor(x => x.MaterialCategoryId).NotEmpty();
        RuleFor(x => x.MaterialAttributeDefinitionId).NotEmpty();
    }
}
