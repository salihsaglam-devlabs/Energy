using FluentValidation;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Requests;

namespace Energy.Application.Modules.Core.UnitOfMeasure.Validators;

/// <summary>UpdateUnitOfMeasureRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateUnitOfMeasureRequestValidator : AbstractValidator<UpdateUnitOfMeasureRequest>
{
    public UpdateUnitOfMeasureRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
