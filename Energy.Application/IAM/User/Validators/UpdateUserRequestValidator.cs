using FluentValidation;
using Energy.Shared.Models.V1.IAM.User.Requests;

namespace Energy.Application.IAM.User.Validators;

/// <summary>UpdateUserRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}
