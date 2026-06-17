using FluentValidation;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Requests;

namespace Energy.Application.FieldOperations.DailySiteReportMaterial.Validators;

/// <summary>UpdateDailySiteReportMaterialRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateDailySiteReportMaterialRequestValidator : AbstractValidator<UpdateDailySiteReportMaterialRequest>
{
    public UpdateDailySiteReportMaterialRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DailySiteReportId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
