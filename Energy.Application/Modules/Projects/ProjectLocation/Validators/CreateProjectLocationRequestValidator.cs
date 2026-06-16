using FluentValidation;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Requests;

namespace Energy.Application.Modules.Projects.ProjectLocation.Validators;

/// <summary>CreateProjectLocationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateProjectLocationRequestValidator : AbstractValidator<CreateProjectLocationRequest>
{
    public CreateProjectLocationRequestValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
