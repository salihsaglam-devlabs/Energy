using FluentValidation;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Requests;

namespace Energy.Application.Core.UnitOfMeasure.Validators;

/// <summary>CreateUnitOfMeasureRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateUnitOfMeasureRequestValidator : AbstractValidator<CreateUnitOfMeasureRequest>
{
    public CreateUnitOfMeasureRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
