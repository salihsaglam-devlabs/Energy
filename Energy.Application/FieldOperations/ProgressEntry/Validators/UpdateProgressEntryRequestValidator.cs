using FluentValidation;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Requests;

namespace Energy.Application.FieldOperations.ProgressEntry.Validators;

/// <summary>UpdateProgressEntryRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateProgressEntryRequestValidator : AbstractValidator<UpdateProgressEntryRequest>
{
    public UpdateProgressEntryRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}
