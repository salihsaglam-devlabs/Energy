using FluentValidation;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Requests;

namespace Energy.Application.Catalog.MaterialCategory.Validators;

/// <summary>UpdateMaterialCategoryRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateMaterialCategoryRequestValidator : AbstractValidator<UpdateMaterialCategoryRequest>
{
    public UpdateMaterialCategoryRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
