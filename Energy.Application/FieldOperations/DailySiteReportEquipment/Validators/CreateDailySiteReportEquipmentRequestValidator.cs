using FluentValidation;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Requests;

namespace Energy.Application.FieldOperations.DailySiteReportEquipment.Validators;

/// <summary>CreateDailySiteReportEquipmentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateDailySiteReportEquipmentRequestValidator : AbstractValidator<CreateDailySiteReportEquipmentRequest>
{
    public CreateDailySiteReportEquipmentRequestValidator()
    {
        RuleFor(x => x.DailySiteReportId).NotEmpty();
    }
}
