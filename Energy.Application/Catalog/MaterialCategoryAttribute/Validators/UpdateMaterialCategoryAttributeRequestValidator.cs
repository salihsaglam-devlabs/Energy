using FluentValidation;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Requests;

namespace Energy.Application.Catalog.MaterialCategoryAttribute.Validators;

/// <summary>UpdateMaterialCategoryAttributeRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateMaterialCategoryAttributeRequestValidator : AbstractValidator<UpdateMaterialCategoryAttributeRequest>
{
    public UpdateMaterialCategoryAttributeRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.MaterialCategoryId).NotEmpty();
        RuleFor(x => x.MaterialAttributeDefinitionId).NotEmpty();
    }
}
