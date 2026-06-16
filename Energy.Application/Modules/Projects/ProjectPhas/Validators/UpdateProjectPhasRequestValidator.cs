using FluentValidation;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Requests;

namespace Energy.Application.Modules.Projects.ProjectPhas.Validators;

/// <summary>UpdateProjectPhasRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateProjectPhasRequestValidator : AbstractValidator<UpdateProjectPhasRequest>
{
    public UpdateProjectPhasRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
