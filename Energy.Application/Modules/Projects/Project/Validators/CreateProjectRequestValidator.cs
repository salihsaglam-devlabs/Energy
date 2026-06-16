using FluentValidation;
using Energy.Shared.Models.V1.Projects.Project.Requests;

namespace Energy.Application.Modules.Projects.Project.Validators;

/// <summary>CreateProjectRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.ProjectTypeId).NotEmpty();
        RuleFor(x => x.StatusId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
