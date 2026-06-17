using FluentValidation;
using Energy.Shared.Models.V1.Core.Department.Requests;

namespace Energy.Application.Core.Department.Validators;

/// <summary>CreateDepartmentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentRequestValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
