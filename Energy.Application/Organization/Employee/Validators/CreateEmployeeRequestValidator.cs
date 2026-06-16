using FluentValidation;
using Energy.Shared.Models.V1.Organization.Employee.Requests;

namespace Energy.Application.Organization.Employee.Validators;

/// <summary>CreateEmployeeRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRequestValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}
