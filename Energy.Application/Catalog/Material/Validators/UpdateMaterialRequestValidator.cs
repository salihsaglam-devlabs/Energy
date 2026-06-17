using FluentValidation;
using Energy.Shared.Models.V1.Catalog.Material.Requests;

namespace Energy.Application.Catalog.Material.Validators;

/// <summary>UpdateMaterialRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateMaterialRequestValidator : AbstractValidator<UpdateMaterialRequest>
{
    public UpdateMaterialRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.MaterialCategoryId).NotEmpty();
        RuleFor(x => x.BaseUnitOfMeasureId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
