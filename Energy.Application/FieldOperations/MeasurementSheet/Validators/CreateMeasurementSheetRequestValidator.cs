using FluentValidation;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Requests;

namespace Energy.Application.FieldOperations.MeasurementSheet.Validators;

/// <summary>CreateMeasurementSheetRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateMeasurementSheetRequestValidator : AbstractValidator<CreateMeasurementSheetRequest>
{
    public CreateMeasurementSheetRequestValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.SheetNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
