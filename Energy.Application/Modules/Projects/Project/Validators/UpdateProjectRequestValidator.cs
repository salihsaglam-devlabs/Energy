using FluentValidation;
using Energy.Shared.Models.V1.Projects.Project.Requests;

namespace Energy.Application.Modules.Projects.Project.Validators;

/// <summary>UpdateProjectRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateProjectRequestValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.ProjectTypeId).NotEmpty();
        RuleFor(x => x.StatusId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
