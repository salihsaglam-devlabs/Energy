using FluentValidation;
using Energy.Shared.Models.V1.Catalog.Material.Requests;

namespace Energy.Application.Modules.Catalog.Material.Validators;

/// <summary>CreateMaterialRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateMaterialRequestValidator : AbstractValidator<CreateMaterialRequest>
{
    public CreateMaterialRequestValidator()
    {
        RuleFor(x => x.MaterialCategoryId).NotEmpty();
        RuleFor(x => x.BaseUnitOfMeasureId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
