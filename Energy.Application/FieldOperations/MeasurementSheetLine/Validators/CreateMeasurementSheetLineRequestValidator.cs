using FluentValidation;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Requests;

namespace Energy.Application.FieldOperations.MeasurementSheetLine.Validators;

/// <summary>CreateMeasurementSheetLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateMeasurementSheetLineRequestValidator : AbstractValidator<CreateMeasurementSheetLineRequest>
{
    public CreateMeasurementSheetLineRequestValidator()
    {
        RuleFor(x => x.MeasurementSheetId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
