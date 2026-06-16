using FluentValidation;
using Energy.Shared.Models.V1.Core.Department.Requests;

namespace Energy.Application.Core.Department.Validators;

/// <summary>UpdateDepartmentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateDepartmentRequestValidator : AbstractValidator<UpdateDepartmentRequest>
{
    public UpdateDepartmentRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
