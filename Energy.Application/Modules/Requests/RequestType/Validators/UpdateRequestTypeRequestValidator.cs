using FluentValidation;
using Energy.Shared.Models.V1.Requests.RequestType.Requests;

namespace Energy.Application.Modules.Requests.RequestType.Validators;

/// <summary>UpdateRequestTypeRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateRequestTypeRequestValidator : AbstractValidator<UpdateRequestTypeRequest>
{
    public UpdateRequestTypeRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Category).NotEmpty();
    }
}
