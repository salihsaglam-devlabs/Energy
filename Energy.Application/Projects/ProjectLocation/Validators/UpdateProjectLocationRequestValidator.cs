using FluentValidation;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Requests;

namespace Energy.Application.Projects.ProjectLocation.Validators;

/// <summary>UpdateProjectLocationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateProjectLocationRequestValidator : AbstractValidator<UpdateProjectLocationRequest>
{
    public UpdateProjectLocationRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
