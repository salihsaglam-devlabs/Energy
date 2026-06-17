using FluentValidation;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Requests;

namespace Energy.Application.FieldOperations.DailySiteReportMaterial.Validators;

/// <summary>CreateDailySiteReportMaterialRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateDailySiteReportMaterialRequestValidator : AbstractValidator<CreateDailySiteReportMaterialRequest>
{
    public CreateDailySiteReportMaterialRequestValidator()
    {
        RuleFor(x => x.DailySiteReportId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
