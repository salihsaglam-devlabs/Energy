using FluentValidation;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Requests;

namespace Energy.Application.Modules.FieldOperations.MeasurementSheetLine.Validators;

/// <summary>UpdateMeasurementSheetLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateMeasurementSheetLineRequestValidator : AbstractValidator<UpdateMeasurementSheetLineRequest>
{
    public UpdateMeasurementSheetLineRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.MeasurementSheetId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
