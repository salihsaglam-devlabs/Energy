using FluentValidation;

namespace Energy.Application.Identity.Auth.Queries.ValidateCredentials;

public sealed class ValidateCredentialsQueryValidator : AbstractValidator<ValidateCredentialsQuery>
{
    public ValidateCredentialsQueryValidator()
    {
        RuleFor(x => x.Request.UserNameOrEmail).NotEmpty();
        RuleFor(x => x.Request.Password).NotEmpty();
    }
}
