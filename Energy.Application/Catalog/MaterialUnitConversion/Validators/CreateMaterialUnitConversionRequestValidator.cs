using FluentValidation;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Requests;

namespace Energy.Application.Catalog.MaterialUnitConversion.Validators;

/// <summary>CreateMaterialUnitConversionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateMaterialUnitConversionRequestValidator : AbstractValidator<CreateMaterialUnitConversionRequest>
{
    public CreateMaterialUnitConversionRequestValidator()
    {
        RuleFor(x => x.MaterialId).NotEmpty();
        RuleFor(x => x.FromUnitOfMeasureId).NotEmpty();
        RuleFor(x => x.ToUnitOfMeasureId).NotEmpty();
    }
}
