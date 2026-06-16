using FluentValidation;
using Energy.Shared.Models.V1.IAM.UserSetting.Requests;

namespace Energy.Application.Modules.IAM.UserSetting.Validators;

/// <summary>CreateUserSettingRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateUserSettingRequestValidator : AbstractValidator<CreateUserSettingRequest>
{
    public CreateUserSettingRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Theme).NotEmpty();
    }
}
