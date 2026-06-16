using FluentValidation;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Requests;

namespace Energy.Application.Modules.Catalog.MaterialUnitConversion.Validators;

/// <summary>UpdateMaterialUnitConversionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateMaterialUnitConversionRequestValidator : AbstractValidator<UpdateMaterialUnitConversionRequest>
{
    public UpdateMaterialUnitConversionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
        RuleFor(x => x.FromUnitOfMeasureId).NotEmpty();
        RuleFor(x => x.ToUnitOfMeasureId).NotEmpty();
    }
}
