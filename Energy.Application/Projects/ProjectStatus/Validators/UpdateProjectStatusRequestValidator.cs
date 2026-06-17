using FluentValidation;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Requests;

namespace Energy.Application.Projects.ProjectStatus.Validators;

/// <summary>UpdateProjectStatusRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateProjectStatusRequestValidator : AbstractValidator<UpdateProjectStatusRequest>
{
    public UpdateProjectStatusRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
