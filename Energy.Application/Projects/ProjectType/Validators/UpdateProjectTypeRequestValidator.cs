using FluentValidation;
using Energy.Shared.Models.V1.Projects.ProjectType.Requests;

namespace Energy.Application.Projects.ProjectType.Validators;

/// <summary>UpdateProjectTypeRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateProjectTypeRequestValidator : AbstractValidator<UpdateProjectTypeRequest>
{
    public UpdateProjectTypeRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
