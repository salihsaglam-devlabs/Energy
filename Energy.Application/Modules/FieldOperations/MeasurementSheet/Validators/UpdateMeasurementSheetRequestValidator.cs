using FluentValidation;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Requests;

namespace Energy.Application.Modules.FieldOperations.MeasurementSheet.Validators;

/// <summary>UpdateMeasurementSheetRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateMeasurementSheetRequestValidator : AbstractValidator<UpdateMeasurementSheetRequest>
{
    public UpdateMeasurementSheetRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.SheetNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
