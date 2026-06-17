using FluentValidation;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Requests;

namespace Energy.Application.Projects.ProjectStatus.Validators;

/// <summary>CreateProjectStatusRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateProjectStatusRequestValidator : AbstractValidator<CreateProjectStatusRequest>
{
    public CreateProjectStatusRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
