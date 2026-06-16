using FluentValidation;
using Energy.Shared.Models.V1.Projects.ProjectType.Requests;

namespace Energy.Application.Modules.Projects.ProjectType.Validators;

/// <summary>CreateProjectTypeRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateProjectTypeRequestValidator : AbstractValidator<CreateProjectTypeRequest>
{
    public CreateProjectTypeRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
