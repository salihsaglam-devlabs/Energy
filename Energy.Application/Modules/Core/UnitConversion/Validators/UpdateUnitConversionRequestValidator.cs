using FluentValidation;
using Energy.Shared.Models.V1.Core.UnitConversion.Requests;

namespace Energy.Application.Modules.Core.UnitConversion.Validators;

/// <summary>UpdateUnitConversionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateUnitConversionRequestValidator : AbstractValidator<UpdateUnitConversionRequest>
{
    public UpdateUnitConversionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FromUnitOfMeasureId).NotEmpty();
        RuleFor(x => x.ToUnitOfMeasureId).NotEmpty();
    }
}
