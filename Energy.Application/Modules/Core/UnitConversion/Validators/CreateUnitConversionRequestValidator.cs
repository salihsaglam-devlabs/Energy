using FluentValidation;
using Energy.Shared.Models.V1.Core.UnitConversion.Requests;

namespace Energy.Application.Modules.Core.UnitConversion.Validators;

/// <summary>CreateUnitConversionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateUnitConversionRequestValidator : AbstractValidator<CreateUnitConversionRequest>
{
    public CreateUnitConversionRequestValidator()
    {
        RuleFor(x => x.FromUnitOfMeasureId).NotEmpty();
        RuleFor(x => x.ToUnitOfMeasureId).NotEmpty();
    }
}
