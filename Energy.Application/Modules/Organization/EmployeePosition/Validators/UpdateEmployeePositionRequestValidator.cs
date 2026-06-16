using FluentValidation;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Requests;

namespace Energy.Application.Modules.Organization.EmployeePosition.Validators;

/// <summary>UpdateEmployeePositionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateEmployeePositionRequestValidator : AbstractValidator<UpdateEmployeePositionRequest>
{
    public UpdateEmployeePositionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
