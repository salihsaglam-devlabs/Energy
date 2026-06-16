using FluentValidation;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Requests;

namespace Energy.Application.Catalog.MaterialCategory.Validators;

/// <summary>CreateMaterialCategoryRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateMaterialCategoryRequestValidator : AbstractValidator<CreateMaterialCategoryRequest>
{
    public CreateMaterialCategoryRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
