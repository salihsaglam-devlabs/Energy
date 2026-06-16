using FluentValidation;
using Energy.Shared.Models.V1.IAM.User.Requests;

namespace Energy.Application.IAM.User.Validators;

/// <summary>CreateUserRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}
