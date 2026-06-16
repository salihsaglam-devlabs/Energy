using FluentValidation;
using Energy.Shared.Models.V1.IAM.UserSetting.Requests;

namespace Energy.Application.Modules.IAM.UserSetting.Validators;

/// <summary>UpdateUserSettingRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateUserSettingRequestValidator : AbstractValidator<UpdateUserSettingRequest>
{
    public UpdateUserSettingRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Theme).NotEmpty();
    }
}
