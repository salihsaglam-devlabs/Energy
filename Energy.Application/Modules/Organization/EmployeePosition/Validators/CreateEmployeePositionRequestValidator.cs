using FluentValidation;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Requests;

namespace Energy.Application.Modules.Organization.EmployeePosition.Validators;

/// <summary>CreateEmployeePositionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateEmployeePositionRequestValidator : AbstractValidator<CreateEmployeePositionRequest>
{
    public CreateEmployeePositionRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
