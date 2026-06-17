using FluentValidation;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Requests;

namespace Energy.Application.FieldOperations.DailySiteReportEquipment.Validators;

/// <summary>UpdateDailySiteReportEquipmentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateDailySiteReportEquipmentRequestValidator : AbstractValidator<UpdateDailySiteReportEquipmentRequest>
{
    public UpdateDailySiteReportEquipmentRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DailySiteReportId).NotEmpty();
    }
}
