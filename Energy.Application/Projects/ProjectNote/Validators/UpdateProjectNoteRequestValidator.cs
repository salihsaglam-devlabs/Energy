using FluentValidation;
using Energy.Shared.Models.V1.Projects.ProjectNote.Requests;

namespace Energy.Application.Projects.ProjectNote.Validators;

/// <summary>UpdateProjectNoteRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateProjectNoteRequestValidator : AbstractValidator<UpdateProjectNoteRequest>
{
    public UpdateProjectNoteRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
    }
}
