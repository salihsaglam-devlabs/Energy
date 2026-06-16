using FluentValidation;
using Energy.Shared.Models.V1.Organization.Employee.Requests;

namespace Energy.Application.Modules.Organization.Employee.Validators;

/// <summary>UpdateEmployeeRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}
