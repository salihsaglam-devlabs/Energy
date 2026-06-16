using FluentValidation;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Requests;

namespace Energy.Application.Notifications.NotificationPreference.Validators;

/// <summary>UpdateNotificationPreferenceRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateNotificationPreferenceRequestValidator : AbstractValidator<UpdateNotificationPreferenceRequest>
{
    public UpdateNotificationPreferenceRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.NotificationType).NotEmpty();
    }
}
